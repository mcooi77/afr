#define HWSERIAL Serial1

const int numBits = 12;             //Number of bits in ADC (resolution)
const int comparatorPin = 23;       //Pin number of comparator out
const int clockPin = 22;            //Pin number of external clock (square wave) input; expecting clock with 50% duty-cycle
const int enablePin = 20;
const int upDownPin = 21;
const int loadPin = 19;


volatile bool clockState = 0;       //Is clock high or low?
volatile bool isSameVal = false;    //Is the value of the master count the same value as last cycle?
volatile bool upDown = 0;     //Init up/down count to up (being low state, or false)
volatile byte loopCounter = 0;  //Keep track of number of clock pulses / loop iteration

byte masterBinCountIn[12];     //Array to store binary value of the master count
byte inputPins[12];         //Array to store the numbers associated with the output pin numbers
short maxValue = 4096;           //Initialize max counter value to zero

bool comparatorState = 0;   //Init comparator state
int masterCount = 0;       //Init master Count
bool inhibited = false;   //Define whether or not counters are told to hold the count for current iteration




void setup() {
  Serial.begin(115200);       //Begin Serial transmission (for troubleshooting)
  HWSERIAL.begin(115200, SERIAL_8N1);
  
  delay(5000);
  
  defineInPins();  //Assign pin numbers for output pins

  pinMode(comparatorPin, INPUT);   //Comparator Pin
  pinMode(clockPin, INPUT);     //Clock-in pin

  pinMode(enablePin, OUTPUT);     //Default to up-count (both low)
  pinMode(upDownPin, OUTPUT);
  digitalWrite(enablePin, LOW);
  digitalWrite(upDownPin, LOW);

  delay(5000);

  
  presetZero();

/* THIS FUNCTION IS FOR TROUBLESHOOTING!!
   binaryToPins();
   
   for(int c = 0; c < 12; c++) {
      Serial.println(masterBinCountIn[c]);
   }
*/ 

  attachInterrupt(clockPin, doOnClock, RISING);   //When clock is on a positive-edge execute doOnClock function
}




void loop() {

}


/******************************************************************
 * THIS FUNCTION DEFINES THE DIGITAL INPUT PINS ON THE TEENSY 3.2
 *****************************************************************/
void defineInPins() {   //Assign pin numbers to parallel binary out: Teensy 3.2 pin #2 --> #14 
  for(byte i = 2; i < (numBits + 2); i++) {
    inputPins[(i-2)] = {i};      //Assign current pin this value
    pinMode(i, INPUT);          //Define pins as outputs
    //Serial.print(inputPins[(i-2)]);
  }
}




/******************************************************************
 * THIS FUNCTION CONTROLS ACTIONS TAKEN ON RISING-EDGE CLOCK PULSE
 ******************************************************************/
void doOnClock() {  
  //Serial.println("clock pulsed!");

  digitalWrite(enablePin, LOW);
  
  
  binaryToPins();                    //Get current binary count
  masterCount = convertBinToDec();   //Convert to decimal value

  comparatorState = digitalRead(comparatorPin);   //Check comparator state


  Serial.println(masterCount);
  
  HWSERIAL.println(masterCount);      //Send the decimal value to external Serial device (pins: #0, #1)

  if(comparatorState == 1) {                //Check if comparator is HIGH
      //if(masterCount == (maxValue - 1)) {   //Check if current count value is at "ceiling" (max)
      if(masterCount == 4095) {
        upDown = 1;
        toggleClk(upDown);           //Control counter IC
        //digitalWrite(enablePin, HIGH);      //Inhibit counting! (See 74LS191N datasheet)
        //inhibited = true;
      }
      else {                  //Else, increment count up
        upDown = 0;
        toggleClk(upDown);    //Control counter IC
      }
  }
  else if(comparatorState == 0) {       //Check if comparator is LOW
      if(masterCount == 0) {            //Check if current count value is at "floor"
        //digitalWrite(enablePin, HIGH);  //Inhibit counting! (See 74LS191N datasheet)
        inhibited = true;
      }
      else {
        upDown = 1;
        toggleClk(upDown);           //Control counter IC
      }
  }
/*
  if(loopCounter < 1000)
  {
    loopCounter++;
  }
  else
  {
    HWSERIAL.println(masterCount);      //Send the decimal value to external Serial device (pins: #0, #1)
    Serial.println(masterCount);
    loopCounter = 0;
  }
*/
}



/*************************************************************************************
 * THIS FUNCTION CONVERTS THE 12-BIT BINARY SEQUENCE STORED IN ARRAY TO DECIMAL VALUE 
 *************************************************************************************/
int convertBinToDec() {
  int result = 0;
  for(int i = 0; i < numBits; i++) {
    result |= masterBinCountIn[(11-i)] << i;
  }
  return result;
}



/******************************************************************************
 * THIS FUNCTION READS THE 12-BIT BINARY VALUE FROM TEENSY 3.2 PINS #2 --> #13
 ******************************************************************************/

void binaryToPins() {  
  for(int e = 0; e < numBits; e++) {
    masterBinCountIn[(11-e)] = digitalRead(inputPins[e]);   //Read our master binary value to output pins
    //Serial.print(masterBinCountIn[e]);
  }
  //Serial.println();
}



/*********************************************************************
 * THIS FUNCTION TOGGLES COUNTING UP/DOWN WITH THE 74LS191N COUNTER 
 *********************************************************************/
void toggleClk(bool hiOrLo) {
  
  if(hiOrLo == 1) {
    if(isSameVal == false) {
      //Count down from up
      digitalWrite(enablePin, HIGH);
      delayMicroseconds(10);
      digitalWrite(upDownPin, HIGH);
      delayMicroseconds(5);
      digitalWrite(enablePin, LOW);
      //Serial.println("down count");
      
      isSameVal = true;
    }
    else {
      //Do nothing
    }
  }
  
  else if(hiOrLo == 0) {
    //Count up from down
    if(isSameVal == true) {
      digitalWrite(enablePin, HIGH);
      delayMicroseconds(10);
      digitalWrite(upDownPin, LOW);
      delayMicroseconds(5);
      digitalWrite(enablePin, LOW);
      //Serial.println("up count");

      isSameVal = false;
    }
     else {
       //Do nothing 
     }
  }
}



/*
 * THIS FUNCTION PRESETS THE COUNTERS TO ZERO!
 */
void presetZero() {
   pinMode(loadPin, OUTPUT);
   digitalWrite(loadPin, HIGH);
   delay(1);
   digitalWrite(loadPin, LOW);
   delay(1);
   digitalWrite(loadPin, HIGH);
}
