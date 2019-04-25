using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Enumeration;
using Windows.UI.ViewManagement;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Text.Core;


namespace SerialSample
{    
    public sealed partial class MainPage : Page
    {
        //Calibration coeff for load cell
        double CorrectionCoeff = 0.01;

        private SerialDevice serialPort = null;
        DataWriter dataWriteObject = null;
        DataReader dataReaderObject = null;

        //Ensure textboxes are set to auto-trigger onscreen keyboard:
        public CoreTextInputPaneDisplayPolicy InputPaneDisplayPolicy = 0;

        //Create list of available Serial UART devices:
        private ObservableCollection<DeviceInformation> listOfDevices;

        //Create list of "data points" for measured fluid:
        ObservableCollection<AFRDataPoint> ListOfFluidDataPts = new ObservableCollection<AFRDataPoint>();

        //Setup vars to handle averaging of incomming data:
        static int ListCtr = 0;
        static List<double> AvgIncommingData = new List<double>(100);

        //Create my Serial Port cancellation token for exiting Serial Comm.:
        private CancellationTokenSource ReadCancellationTokenSource;

        //Create my application timers:
        public DateTime Publictime = DateTime.Now;
        DispatcherTimer timer;
        DispatcherTimer StartTreatmentTimer;

        //Update Urine data point(s) timer:
        DateTimeOffset startTime;
        DateTimeOffset lastTime;

        //Track time since treatment started timer:
        DateTimeOffset TreatmentStartTime;
        DateTimeOffset TreatmentLastTime;

        //Time elapsed since treatment began:
        TimeSpan TreatmentTimeDelta;

        //Ongoing average of the rate of urine produced per hour:
        double OngoingRate = 0;

        //public string LoadCellInputData = string.Empty;
        double realAvgValue = 0;

        //Flags to trigger various events:
        public bool IsInvalidEntry = false;
        public bool BeginTreatmentTracking = false;


        public MainPage()
        {
            this.InitializeComponent();            
            comPortInput.IsEnabled = false;
            //sendTextButton.IsEnabled = false;
            
            StartTimer();

            //CreateListItem();
            
            listOfDevices = new ObservableCollection<DeviceInformation>();
            ListAvailablePorts();
        }

        /// <summary>
        /// ListAvailablePorts
        /// - Use SerialDevice.GetDeviceSelector to enumerate all serial devices
        /// - Attaches the DeviceInformation to the ListBox source so that DeviceIds are displayed
        /// </summary>
        private async void ListAvailablePorts()
        {
            try
            {
                string aqs = SerialDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs);

                status.Text = "Select a device and connect";

                for (int i = 0; i < dis.Count; i++)
                {
                    listOfDevices.Add(dis[i]);
                }

                DeviceListSource.Source = listOfDevices;
                comPortInput.IsEnabled = true;
                ConnectDevices.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                status.Text = ex.Message;
            }

            //InitSerial();
        }

        /// <summary>
        /// comPortInput_Click: Action to take when 'Connect' button is clicked
        /// - Get the selected device index and use Id to create the SerialDevice object
        /// - Configure default settings for the serial port
        /// - Create the ReadCancellationTokenSource token
        /// - Start listening on the serial port input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        //private async void InitSerial()
        private async void comPortInput_Click(object sender, RoutedEventArgs e)
        {
            var selection = ConnectDevices.SelectedItems;

            if (selection.Count <= 0)
            {
                status.Text = "Select a device and connect";
                return;
            }

           DeviceInformation entry = (DeviceInformation)selection[0];         

            try
            {                
                serialPort = await SerialDevice.FromIdAsync(entry.Id);

                if (serialPort == null) return;

                // Disable the 'Connect' button 
                comPortInput.IsEnabled = false;

                // Configure serial settings
                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1);                
                serialPort.BaudRate = 115200;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;
                serialPort.Handshake = SerialHandshake.None;

                // Display configured settings
                status.Text = "Serial port configured successfully: ";
                status.Text += serialPort.BaudRate + "-";
                status.Text += serialPort.DataBits + "-";
                status.Text += serialPort.Parity.ToString() + "-";
                status.Text += serialPort.StopBits;

                // Set the RcvdText field to invoke the TextChanged callback
                // The callback launches an async Read task to wait for data
                rcvdText.Text = "Waiting for data...";

                // Create cancellation token object to close I/O operations when closing the device
                ReadCancellationTokenSource = new CancellationTokenSource();

                // Enable 'WRITE' button to allow sending data
                //sendTextButton.IsEnabled = true;

                Listen();
            }
            catch (Exception ex)
            {
                status.Text = ex.Message;
                comPortInput.IsEnabled = true;
                //sendTextButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// sendTextButton_Click: Action to take when 'WRITE' button is clicked
        /// - Create a DataWriter object with the OutputStream of the SerialDevice
        /// - Create an async task that performs the write operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /*
        private async void sendTextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                if (serialPort != null)
                {
                    // Create the DataWriter object and attach to OutputStream
                    dataWriteObject = new DataWriter(serialPort.OutputStream);

                    //Launch the WriteAsync task to perform the write
                    //await WriteAsync();
                }
                else
                {
                    status.Text = "Select a device and connect";                
                }
            }
            catch (Exception ex)
            {
                status.Text = "sendTextButton_Click: " + ex.Message;
            }
            finally
            {
                // Cleanup once complete
                if (dataWriteObject != null)
                {
                    dataWriteObject.DetachStream();
                    dataWriteObject = null;
                }
            }
        }
        */

        /// <summary>
        /// WriteAsync: Task that asynchronously writes data from the input text box 'sendText' to the OutputStream 
        /// </summary>
        /// <returns></returns>
        ///
        /*
        private async Task WriteAsync()
        {
            Task<UInt32> storeAsyncTask;

            if (sendText.Text.Length != 0)
            {
                // Load the text from the sendText input text box to the dataWriter object
                dataWriteObject.WriteString(sendText.Text);                

                // Launch an async task to complete the write operation
                storeAsyncTask = dataWriteObject.StoreAsync().AsTask();

                UInt32 bytesWritten = await storeAsyncTask;
                if (bytesWritten > 0)
                {                    
                    status.Text = sendText.Text + ", ";
                    status.Text += "bytes written successfully!";
                }
                sendText.Text = "";
            }
            else
            {
                status.Text = "Enter the text you want to write and then click on 'WRITE'";
            }
        }
        */
        /// <summary>
        /// - Create a DataReader object
        /// - Create an async task to read from the SerialDevice InputStream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Listen()
        {
            try
            {
                if (serialPort != null)
                {
                    dataReaderObject = new DataReader(serialPort.InputStream);

                    // keep reading the serial input
                    while (true)
                    {
                        await ReadAsync(ReadCancellationTokenSource.Token);
                    }
                }
            }
            catch (TaskCanceledException tce) 
            {
                status.Text = "Reading task was cancelled, closing device and cleaning up";
                CloseDevice();            
            }
            catch (Exception ex)
            {
                status.Text = ex.Message;
            }
            finally
            {
                // Cleanup once complete
                if (dataReaderObject != null)
                {
                    dataReaderObject.DetachStream();
                    dataReaderObject = null;
                }
            }
        }

        /// <summary>
        /// ReadAsync: Task that waits on data and reads asynchronously from the serial device InputStream
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            Task<UInt32> loadAsyncTask;

            uint ReadBufferLength = 1024;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

            using (var childCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                // Create a task object to wait for data on the serialPort.InputStream
                loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(childCancellationTokenSource.Token);

                // Launch the task and wait
                UInt32 bytesRead = await loadAsyncTask;
                if (bytesRead > 0)
                {
                    rcvdText.Text = dataReaderObject.ReadString(bytesRead);
                    int CountData = int.Parse(rcvdText.Text);
                    double ScaledData = CountData * CorrectionCoeff;
                    //LoadCellInputData = rcvdText.Text;

                    //string[] str = rcvdText.Text.Split(new char[] {'\r', '\n'});
                    AvgIncommingData.Add(ScaledData);
                    ListCtr++;

                    if (ListCtr == 100)
                    {
                        double RunningTotal = 0;

                        //Calcuate avg:
                        for (int j=0; j < 100; j++)
                        {
                            RunningTotal += AvgIncommingData[j];
                        }

                        string val = RunningTotal.ToString();
                        realAvgValue = RunningTotal / 100;

                        ListCtr = 0;
                        AvgIncommingData.RemoveRange(0, 100);
                    }

                    //status.Text = "bytes read successfully!";
                    
                }
            }
        }

        /// <summary>
        /// CancelReadTask:
        /// - Uses the ReadCancellationTokenSource to cancel read operations
        /// </summary>
        private void CancelReadTask()
        {         
            if (ReadCancellationTokenSource != null)
            {
                if (!ReadCancellationTokenSource.IsCancellationRequested)
                {
                    ReadCancellationTokenSource.Cancel();
                }
            }         
        }

        /// <summary>
        /// CloseDevice:
        /// - Disposes SerialDevice object
        /// - Clears the enumerated device Id list
        /// </summary>
        private void CloseDevice()
        {            
            if (serialPort != null)
            {
                serialPort.Dispose();
            }
            serialPort = null;

            comPortInput.IsEnabled = true;
            //sendTextButton.IsEnabled = false;            
            rcvdText.Text = "";
            listOfDevices.Clear();               
        }

        /// <summary>
        /// closeDevice_Click: Action to take when 'Disconnect and Refresh List' is clicked on
        /// - Cancel all read operations
        /// - Close and dispose the SerialDevice object
        /// - Enumerate connected devices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeDevice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                status.Text = "";
                CancelReadTask();
                CloseDevice();
                ListAvailablePorts();
            }
            catch (Exception ex)
            {
                status.Text = ex.Message;
            }          
        }

        private void ConnectDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SendText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        /*START MY CODE*/
        //
        //
        private void CreateListItem()
        {
            CalculateAvgUrine();

            ListOfFluidDataPts.Add(new AFRDataPoint(Publictime, realAvgValue.ToString("0.##") + " mL", OngoingRate.ToString("0.##") + " mL/kg/h"));


            //listofmydatapoints = MyList;
            // DeviceListSource.Source = listOfDevices;
            MyDataPtsSource.Source = ListOfFluidDataPts;

            DynamicListView.Focus(FocusState.Programmatic);
            DynamicListView.SelectedIndex = (DynamicListView.Items.Count - 1);
            DynamicListView.ScrollIntoView(DynamicListView.SelectedItem);

            //TEST CODE NEW START
            
            //ListViewItem NewItem = DynamicListView.ItemContainerGenerator.ContainerFromItem(DynamicListView.SelectedItem) as ListViewItem;
            /*
            if (rowFormatToggle == true)
            {
                this.Resources[HighlightBrushKey] = Brushes.Red;
                //NewItem.Background = new SolidColorBrush(Color.FromArgb(255, 248, 203, 173));
                rowFormatToggle = false;
            }
            else
            {
                //NewItem.Background = new SolidColorBrush(Color.FromArgb(255, 252, 228, 214));
                rowFormatToggle = true;
            }
            */
            //END NEW TEST CODE
            
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        public void CalculateAvgUrine()
        {
            //Getting Current/Projected Urine Rate:
            OngoingRate = 3600 * realAvgValue / (TreatmentTimeDelta.TotalSeconds * int.Parse(PatientWeightEnteredBox.Text));
        }

        public void TreatmentTimerBegin()
        {
            StartTreatmentTimer = new DispatcherTimer();
            StartTreatmentTimer.Tick += StartTreatmentTimer_Tick;
            StartTreatmentTimer.Interval = new TimeSpan(0, 0, 1);

            TreatmentStartTime = DateTime.Now;
            TreatmentLastTime = TreatmentStartTime;

            StartTreatmentTimer.Start();
        }

        public void StartTreatmentTimer_Tick(object sender, object e)
        {
            DateTime CurrentTime = DateTime.Now;
            TreatmentTimeDelta = CurrentTime - TreatmentStartTime;

            TimeElapsed.Text = TreatmentTimeDelta.ToString(@"hh\:mm\:%s");
        }

        public void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += DispatchTimer_Tick;
            timer.Interval = new TimeSpan(0, 0, 10);

            //Debugging check:
            //string str = "Msg Timer.IsEnabled = " + timer.IsEnabled;

            startTime = DateTime.Now;
            lastTime = startTime;

            timer.Start();

            //Debugging check:
            //string str1 = "Msg Timer.IsEnabled = " + timer.IsEnabled;
        }

        public void DispatchTimer_Tick(object sender, object e)
        {
            DateTime time = DateTime.Now;
            Publictime = DateTime.Now;
            TimeSpan span = time - lastTime;
            lastTime = time;

            if(BeginTreatmentTracking == true)
            {
                CreateListItem();
            }

            //Debugging Check:
            //string str = noTimesTicked + "\t time since last tick " + span.ToString() + "\n";

            //noTimesTicked++;

            /* Debugging Statement:
            if (noTimesTicked > noTimesToTick)
            {
                stopTime = time;
                string str2 = "Calling dispatcherTime.Stop()";
                timer.Stop();

                span = stopTime - startTime;
                string str3 = "Total Time Start - Stop :" + span.ToString();
            }
            */
        }


        //
        // <Patient Weight Entry Code:>
        //
        private void PatientWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(PatientWeightTxt.Text, "  ^ [0-9]"))
            {
                PatientWeightTxt.Text = "";
            }
        }

        //Get and display entered weight information
        private void EnterWeightBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsInvalidEntry == false)
            {
                PatientWeightEnteredBox.Text = PatientWeightTxt.Text;
            }
        }

        //Display error msg if user enters non-numerical characters
        private void PatientWeightTxt_CharacterReceived(UIElement sender, Windows.UI.Xaml.Input.CharacterReceivedRoutedEventArgs args)
        {
            if (!char.IsControl(args.Character) && !char.IsDigit(args.Character) && (args.Character != '.'))
            {
                args.Handled = true;
                WeightInputErrorTxt.Opacity = 100;
                IsInvalidEntry = true;
            }
            else
            {
                WeightInputErrorTxt.Opacity = 0;
                IsInvalidEntry = false;
            }
        }

        //Try to bring up on-screen keyboard
        private void PatientWeightTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            InputPane currentPane = InputPane.GetForCurrentView();
            currentPane.TryShow();
        }
        //
        // </Patient Weight Entry Code>
        //


        //
        // <Patient Burn Entry Information Code:>
        //
        private void PatientPercentBurnTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(PatientPercentBurnTxt.Text, "  ^ [0-9]"))
            {
                PatientPercentBurnTxt.Text = "";
            }
        }

        //Get and display entered burn information
        private void EnterBurnBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsInvalidEntry == false)
            {
                PatientBurnEnteredBox.Text = PatientPercentBurnTxt.Text;
            }
        }

        //Display error msg if user enters non-numerical characters
        private void PatientPercentBurnTxt_CharacterReceived(UIElement sender, Windows.UI.Xaml.Input.CharacterReceivedRoutedEventArgs args)
        {
            if (!char.IsControl(args.Character) && !char.IsDigit(args.Character) && (args.Character != '.'))
            {
                args.Handled = true;
                PatientBurnInputErrorTxt.Opacity = 100;
                IsInvalidEntry = true;
            }
            else
            {
                PatientBurnInputErrorTxt.Opacity = 0;
                IsInvalidEntry = false;
            }
        }

        //Try to bring up on-screen keyboard
        private void PatientPercentBurnTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            InputPane currentPane = InputPane.GetForCurrentView();
            currentPane.TryShow();
        }
        //
        // </Patient Burn Entry Information Code>
        //


        //
        //Additional Text Boxes:
        //
        private void PatientNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PatientWeightEnteredBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PatientBurnEnteredBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PatientParklandResultBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RecommendedRateEightHrs_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RecommendedRateFinalHrs_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComputeTreatmentBtn_Click(object sender, RoutedEventArgs e)
        {
            double FluidVolResult = ParklandFormula();

            PatientParklandResultBox.Text = FluidVolResult.ToString();

            RecommendedRateEightHrs.Text = ((FluidVolResult / 2) / 8).ToString();

            RecommendedRateFinalHrs.Text = ((FluidVolResult / 2) / 16).ToString();

            BeginTreatmentTracking = true;
            TreatmentTimerBegin();
            TreatmentInitalTime.Text = TreatmentStartTime.ToString(@"dd\.hh\:mm");
        }

        public int ParklandFormula()
        {
            int ParklandResult = 0;
            int Wt = int.Parse(PatientWeightEnteredBox.Text);
            int Percent = int.Parse(PatientBurnEnteredBox.Text);

            ParklandResult = 4 * Wt * Percent;

            return ParklandResult;
        }

        private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class AFRDataPoint
    {        
        public AFRDataPoint(DateTime timestamp, string totalvolume, string estimatedrateperhour)
        {
            this.TimeStamp = timestamp;
            this.TotalVolume = totalvolume;
            this.EstimatedRatePerHr = estimatedrateperhour;
        }

        public DateTime TimeStamp { get; set; }

        public string TotalVolume { get; set; }

        public string EstimatedRatePerHr { get; set; }
    }

}
