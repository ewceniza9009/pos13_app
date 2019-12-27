using System.Collections.ObjectModel;
using System.Globalization;
using Windows.UI;
using Windows.UI.Popups;
using pos13_app.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237
using pos13_app.Models;
using pos13_app.pos13_app_services;

namespace pos13_app
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
 
    public sealed partial class SysSalesOrder : Page
    {
        #region Module Variable Declarations
        public pos13_app_services.Pos13_app_serviceClient service;

        private DispatcherTimer dispatcherTimer;

        public int TableGroupId;
        public int TableGroupPage;
        public int TableGroupPages;
        public int TablePage;
        public int TablePages;

        public DateTime SalesOrderDate;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        /// 
        #endregion

        #region Frame Initialization
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        public SysSalesOrder()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }
        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            service = new Pos13_app_serviceClient();

            TableGroupId = 0;

            GetMinTableGroupId();

            TableGroupPage = 1;
            TableGroupPages = 0;

            TablePage = 1;
            TablePages = 0;
           
            SalesDate.Date = DateTime.Now.Date;
            InvoiceStatus.SelectedIndex = 0;

            FillTableGroup();

            FillTrnSalesOrderList();
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
        #endregion

        #region NavigationHelper Registration
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            DispatcherTimerSetup();
        }
        #endregion

        #region Table Group Buttons
        private void CmdWalkIn_Click(object sender, RoutedEventArgs e)
        {
           TableClick(CmdWalkIn);
        }

        private void CmdDelivery_Click(object sender, RoutedEventArgs e)
        {
            TableClick(CmdDelivery);
        }
        private void CmdTableGroup01_Click(object sender, RoutedEventArgs e)
        {
            TableGroupId = int.Parse(CmdTableGroup01.Tag.ToString());
            FillTable(TableGroupId);
        }
        private void CmdTableGroup02_Click(object sender, RoutedEventArgs e)
        {
            TableGroupId = int.Parse(CmdTableGroup02.Tag.ToString());
            FillTable(TableGroupId);

        }
        private void CmdTableGroup03_Click(object sender, RoutedEventArgs e)
        {
            TableGroupId = int.Parse(CmdTableGroup03.Tag.ToString());
            FillTable(TableGroupId);
        }

        private void CmdTableGroup04_Click(object sender, RoutedEventArgs e)
        {
            TableGroupId = int.Parse(CmdTableGroup04.Tag.ToString());
            FillTable(TableGroupId);
        }

        private void CmdTableGroup05_Click(object sender, RoutedEventArgs e)
        {
            TableGroupId = int.Parse(CmdTableGroup05.Tag.ToString());
            FillTable(TableGroupId);
        }

        private void CmdTableGroup06_Click(object sender, RoutedEventArgs e)
        {
            TableGroupId = int.Parse(CmdTableGroup06.Tag.ToString());
            FillTable(TableGroupId);
        }
        #endregion

        #region Table Navigation Buttons
        private void CmdPreviousTableGroup_Click(object sender, RoutedEventArgs e)
        {
            TableGroupPage--;
            FillTableGroup();
        }

        private void CmdNextTableGroup_Click(object sender, RoutedEventArgs e)
        {
            TableGroupPage++;
            FillTableGroup();
        }
        private void CmdPreviousTable_Click(object sender, RoutedEventArgs e)
        {
            TablePage--;
            FillTable(TableGroupId);
        }
        private void cmdNextTable_Click(object sender, RoutedEventArgs e)
        {
            TablePage++;
            FillTable(TableGroupId);
        }
        #endregion
        
        #region Table Buttons
        private void cmdTable01_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable01);
        }

        private void cmdTable02_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable02);
        }

        private void cmdTable03_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable03);
        }

        private void cmdTable04_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable04);
        }

        private void cmdTable05_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable05);
        }

        private void cmdTable06_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable06);
        }

        private void cmdTable07_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable07);
        }

        private void cmdTable08_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable08);
        }

        private void cmdTable09_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable09);
        }

        private void cmdTable10_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable10);
        }

        private void cmdTable11_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable11);
        }

        private void cmdTable12_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable12);
        }

        private void cmdTable13_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable13);
        }

        private void cmdTable14_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable14);
        }

        private void cmdTable15_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable15);
        }

        private void cmdTable16_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable16);
        }

        private void cmdTable17_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable17);
        }

        private void cmdTable18_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable18);
        }

        private void cmdTable19_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable19);
        }

        private void cmdTable20_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable20);
        }

        private void cmdTable21_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable21);
        }

        private void cmdTable22_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable22);
        }

        private void cmdTable23_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable23);
        }

        private void cmdTable24_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable24);
        }

        private void cmdTable25_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable25);
        }

        private void cmdTable26_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable26);
        }

        private void cmdTable27_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable27);
        }

        private void cmdTable28_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable28);
        }

        private void cmdTable29_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable29);
        }

        private void cmdTable30_Click(object sender, RoutedEventArgs e)
        {
            TableClick(cmdTable30);
        }
        #endregion

        #region On Change Events
        private void InvoiceStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillTrnSalesOrderList();
        }

        private void SalesDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            FillTrnSalesOrderList();
        }
        private void TrnSalesOrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var TrnSalesOrderListSelectionReturn = TrnSalesOrderList.SelectedItem.ToString();

            //var subStringStartIndex = "{ Id = ".Length;
            //var subStringLength = TrnSalesOrderListSelectionReturn.IndexOf(", SalesNumber", System.StringComparison.Ordinal) - subStringStartIndex;
            //var invoiceId = int.Parse((TrnSalesOrderListSelectionReturn.Substring(subStringStartIndex, subStringLength)));

            //this.Frame.Navigate(typeof(TrnSalesOrderDetail),invoiceId);
        }
        #endregion

        #region Control Events
        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var strOut = b.Tag.ToString().Split('-');

            var SIId = int.Parse(strOut[0]);
            var TBId = int.Parse(strOut[1]);

            var param = new TrnSalesParams()
            {
                InvoiceId =SIId,
                TableId = TBId
            };
            this.Frame.Navigate(typeof(TrnSalesOrderDetail), param);
        }

        private async void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;

            var message = new MessageDialog("You have deleted " + b.Tag,"Sales Order");
            message.ShowAsync();

            service = new Pos13_app_serviceClient();

            await service.DeleteSalesAsync(int.Parse(b.Tag.ToString()));

            FillTrnSalesOrderList();
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        #endregion

        #region Functions
        private async void GetMinTableGroupId()
        {
            TableGroupId = await service.MinTableGroupIdAsync();
        }

        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        public async void TableClick(Button ClickedControl)
        {         
            if (ClickedControl.Tag.ToString().Length > 0 )
            {
                if (ClickedControl.Background == null)
                {
                    var param = new TrnSalesParams()
                    {
                        InvoiceId = 0,
                        TableId = int.Parse(ClickedControl.Tag.ToString())
                    };
                    this.Frame.Navigate(typeof(TrnSalesOrderDetail), param);
                }
                else
                {
                    string dat = String.Format("{0:MM/dd/yyyy}", SalesDate.Date).Substring(0, 10);
                    var SalesDateParam = DateTime.Parse(dat);

                    var service = new Pos13_app_serviceClient();

                    var SelectedInvoiceStatus = (ComboBoxItem)InvoiceStatus.SelectedItem;
                    var strSelectedInvoiceStatus = SelectedInvoiceStatus.Content.ToString();

                    var param = new TrnSalesParams()
                    {
                        InvoiceId = await service.GetTableInvoiceIdAsync(SalesDateParam, int.Parse(ClickedControl.Tag.ToString()), strSelectedInvoiceStatus),
                        TableId = int.Parse(ClickedControl.Tag.ToString())
                    };
                    this.Frame.Navigate(typeof(TrnSalesOrderDetail), param);
                }
            }
        }

        private void dispatcherTimer_Tick(object sender, object e)
        {
            FillTable(TableGroupId);
        }
        public async void FillTrnSalesOrderList()
        {
            string dat = String.Format("{0:MM/dd/yyyy}", SalesDate.Date).Substring(0, 10);

            SalesOrderDate = DateTime.Parse(dat);

            var SelectedInvoiceStatus = (ComboBoxItem)InvoiceStatus.SelectedItem;
            var strSelectedInvoiceStatus = SelectedInvoiceStatus.Content.ToString();

            strSelectedInvoiceStatus = strSelectedInvoiceStatus ?? "Open";

            var TrnSalesOrderLists = from i in await service.TrnSalesOrderListSF1Async(SalesOrderDate, strSelectedInvoiceStatus)
                                     select new
                                     {
                                         Id = i.Id,
                                         SalesNumber = i.SalesNumber,
                                         Amount = i.AmountDescription,
                                         TableCode = i.TableCode,
                                         User = i.User,
                                         InvoiceTableId = i.InvoiceTableId
                                     };
            TrnSalesOrderList.DataContext = TrnSalesOrderLists;

        }

        public async void FillTableGroup()
        {
            const int TableGroupNumberOfButtons = 6;

            var MstTableGroup = await service.GetMstTableGroupAsync();

            int MstTableGroupCount = MstTableGroup.Count();

            int indexer = 0;
            int addOnIndexer = 0;

            TableGroupPages = ((MstTableGroupCount - (MstTableGroupCount % TableGroupNumberOfButtons)) /
                          TableGroupNumberOfButtons) +
                         ((MstTableGroupCount % TableGroupNumberOfButtons) > 0 ? 1 : 0);

            TableGroupPage = TableGroupPage < 1 ? 1 : TableGroupPage;
            TableGroupPage = TableGroupPage > TableGroupPages ? TableGroupPages : TableGroupPage;

            addOnIndexer = TableGroupNumberOfButtons * (TableGroupPage - 1);
            addOnIndexer = addOnIndexer == 0 ? 0 : addOnIndexer;

            for (int i = 1; i <= TableGroupNumberOfButtons; i++)
            {
                var buttonName = string.Format("CmdTableGroup{0}", GetRightString((i + 100).ToString(), 2));
                var button = (Button)FindName(buttonName);
                if (button != null)
                    button.Content = (indexer + addOnIndexer) >= MstTableGroupCount
                        ? ""
                        : MstTableGroup[indexer + addOnIndexer].TableGroup;
                if (button != null)
                    button.Tag = (indexer + addOnIndexer) >= MstTableGroupCount
                        ? 0.ToString()
                        : MstTableGroup[indexer + addOnIndexer].Id.ToString();
                indexer++;
            }
        }
        public async void FillTable(int TableGroupId)
        {
            const int TableNumberOfButtons = 30;

            var MstTable = await service.GetMstTableAsync(TableGroupId);

            var SelectedInvoiceStatus = (ComboBoxItem)InvoiceStatus.SelectedItem;
            var strSelectedInvoiceStatus = SelectedInvoiceStatus.Content.ToString();

            strSelectedInvoiceStatus = strSelectedInvoiceStatus ?? "Open";

            var TrnSalesOrderListQ002 = from i in await service.TrnSalesOrderListSF1Async(SalesOrderDate, strSelectedInvoiceStatus)
                                        select new
                                        {
                                            Id = i.Id,
                                            SalesNumber = i.SalesNumber,
                                            Amount = String.Format("{0:c}", i.Amount),
                                            TableCode = i.TableCode,
                                            User = i.User,
                                            InvoiceStatus = i.InvoiceStatus
                                        };

            int MstTableCount = MstTable.Count();

            int indexer = 0;
            int addOnIndexer = 0;

            TablePages = ((MstTableCount - (MstTableCount % TableNumberOfButtons)) / TableNumberOfButtons) +
                         ((MstTableCount % TableNumberOfButtons) > 0 ? 1 : 0);

            TablePage = TablePage < 1 ? 1 : TablePage;
            TablePage = TablePage > (TablePages < 1 ? 1 : TablePages) ? TablePages : TablePage;

            addOnIndexer = TableNumberOfButtons * (TablePage - 1);
            addOnIndexer = addOnIndexer == 0 ? 0 : addOnIndexer;

            var buttonColorIfOpen = new SolidColorBrush(ColorHelper.FromArgb(255, 255, 10, 50));
            var buttonColorIfBilled = new SolidColorBrush(ColorHelper.FromArgb(255,10,255,50));

            for (int i = 1; i <= TableNumberOfButtons; i++)
            {
                var buttonName = string.Format("cmdTable{0}", GetRightString((i + 100).ToString(), 2));
                var button = (Button)FindName(buttonName);

                var invoiceStat = "NA";
                var tableCode = "NA";


                if (button != null)
                {
                    button.Content = (indexer + addOnIndexer) >= MstTableCount
                        ? ""
                        : MstTable[indexer + addOnIndexer].TableCode;

                    button.Tag = (indexer + addOnIndexer) >= MstTableCount
                        ? ""
                        : MstTable[indexer + addOnIndexer].Id.ToString();

                    if (button.Content != null) tableCode = button.Content.ToString() ?? "NA";

                    invoiceStat = (from j in TrnSalesOrderListQ002.ToList()
                                   where j.TableCode != null && j.TableCode == tableCode
                                   select j.InvoiceStatus).FirstOrDefault() ?? "NA";


                    if (invoiceStat == "Open")
                    {
                        if (button != null) button.Background = buttonColorIfOpen;
                    }
                    else if (invoiceStat == "Billed")
                    {
                        if (button != null) button.Background = buttonColorIfBilled;
                    }
                    else if (invoiceStat == "NA")
                    {
                        if (button != null) button.Background = null;
                    }
                }

                indexer++;
            }
        }
        public static string GetRightString(string param, int length)
        {
            string result = param.Substring(param.Length - length, length);
            return result;
        }
        #endregion

    }
}
