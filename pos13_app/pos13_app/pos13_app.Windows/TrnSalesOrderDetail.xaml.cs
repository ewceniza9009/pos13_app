using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using pos13_app.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232
using pos13_app.Models;
using pos13_app.Modules;
using pos13_app.pos13_app_services;
using System.ComponentModel;
using TrnSales = pos13_app.Models.TrnSales;

namespace pos13_app
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class TrnSalesOrderDetail : Page
    {

        #region Module Variable Declaration
        public pos13_app_services.Pos13_app_serviceClient service;

        private DispatcherTimer dispatcherTimer;

        private DateTime InvoiceDate;
        private int InvoiceId = 0;
        private int TableId = 0;

        public int ItemGroupId;
        public int ItemGroupPage;
        public int ItemGroupPages;
        public int ItemPage;
        public int ItemPages;

        private ModCurrentUser CurrentUser;
        private int userId;
        public int SalesLineId;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public TrnSalesOrderDetail()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }
        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            object navigationParameter;
            if (e.PageState != null && e.PageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = e.PageState["SelectedItem"];
            }

            service = new Pos13_app_serviceClient();

            ItemGroupId = 0;

            GetMinItemGroupId();

            ItemGroupPage = 1;
            ItemGroupPages = 0;

            ItemPage = 1;
            ItemPages = 0;

            FillItemGroup();

            // TODO: Assign a bindable group to this.DefaultViewModel["Group"]
            // TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]
            // TODO: Assign the selected item to this.flipView.SelectedItem
        }
        #endregion

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            TrnSalesParams param = (TrnSalesParams) e.Parameter;

            navigationHelper.OnNavigatedTo(e);

            InvoiceId = 0;
            TableId = 0;

            CurrentUser = new ModCurrentUser();

            //CurrentUser.ReadCurrentUser();
            userId = 1; //int.Parse(CurrentUser.LoggedUser);

            if (param != null)
            {
                InvoiceId = int.Parse((param.InvoiceId.ToString()));
                TableId = int.Parse((param.TableId.ToString()));
            }

            if (InvoiceId == 0)
            {
                SaveSalesOrder();
            }

            GetSalesOrderDetail();

            GetSalesOrderDetailItem(InvoiceId);

            DispatcherTimerSetup();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region Item Group Buttons
        private void CmdWalkIn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CmdItemGroup01_Click(object sender, RoutedEventArgs e)
        {
            ItemGroupId = int.Parse(CmdItemGroup01.Tag.ToString());
            FillItem(ItemGroupId);
        }
        private void CmdItemGroup02_Click(object sender, RoutedEventArgs e)
        {
            ItemGroupId = int.Parse(CmdItemGroup02.Tag.ToString());
            FillItem(ItemGroupId);
        }
        private void CmdItemGroup03_Click(object sender, RoutedEventArgs e)
        {
            ItemGroupId = int.Parse(CmdItemGroup03.Tag.ToString());
            FillItem(ItemGroupId);
        }

        private void CmdItemGroup04_Click(object sender, RoutedEventArgs e)
        {
            ItemGroupId = int.Parse(CmdItemGroup04.Tag.ToString());
            FillItem(ItemGroupId);
        }

        private void CmdItemGroup05_Click(object sender, RoutedEventArgs e)
        {
            ItemGroupId = int.Parse(CmdItemGroup05.Tag.ToString());
            FillItem(ItemGroupId);
        }

        private void CmdItemGroup06_Click(object sender, RoutedEventArgs e)
        {
            ItemGroupId = int.Parse(CmdItemGroup06.Tag.ToString());
            FillItem(ItemGroupId);
        }
        #endregion

        #region Item Navigation Buttons
        private void CmdPreviousItemGroup_Click(object sender, RoutedEventArgs e)
        {
            ItemGroupPage--;
            FillItemGroup();
        }

        private void CmdNextItemGroup_Click(object sender, RoutedEventArgs e)
        {
            ItemGroupPage++;
            FillItemGroup();
        }
        private void CmdPreviousItem_Click(object sender, RoutedEventArgs e)
        {
            ItemPage--;
            FillItem(ItemGroupId);
        }

        private void cmdNextItem_Click(object sender, RoutedEventArgs e)
        {
            ItemPage++;
            FillItem(ItemGroupId);
        }

        #endregion

        #region Item Buttons
        private void cmdItem01_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem02_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem03_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem04_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem05_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem06_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem07_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem08_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem09_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem10_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem11_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem12_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem13_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem14_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem15_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem16_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem17_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem18_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem19_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem20_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem21_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem22_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem23_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem24_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem25_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem26_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem27_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem28_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem29_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }

        private void cmdItem30_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var ItemId = int.Parse(b.Tag.ToString());

            EditItemPopUp(0, ItemId);
        }
        #endregion

        #region PopUps
        private void ItemQuantity_Change(object sender,KeyRoutedEventArgs e)
        {
            if (e.Key==VirtualKey.Enter)
            {
                this.ItemQuantity.Text = Math.Abs(decimal.Parse(this.ItemQuantity.Text)).ToString();

                ComputeAmount();
                ComputeVatAmount();
            }
        }
        private void DiscountCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var comboBoxItem = comboBox.ItemContainerGenerator.ContainerFromItem(comboBox.SelectedItem) as ComboBoxItem;
                if (comboBoxItem == null)
                {
                    return;
                }
                var textBlock = FindVisualChildByName<TextBlock>(comboBoxItem, "DiscountIndex");
                var textBlock1 = FindVisualChildByName<TextBlock>(comboBoxItem, "DiscountRateIndex");

                decimal SellingPrice = 0;
                decimal DiscountAmount = 0;


                if (textBlock.Text == "Senior Citizen Discount")
                {
                    this.ItemDiscountRate.Text = String.Format("{0:N}", textBlock1.Text);

                    if (decimal.Parse(ItemVatRate.Text) > 0)
                    {
                        var price = decimal.Parse(this.ItemPrice.Text);
                        var taxrate = decimal.Parse(this.ItemVatRate.Text);

                        SellingPrice = Math.Round((price - ((price/(1 + (taxrate/100)))*(taxrate/100))), 2);
                    }
                    else
                    {
                        var price = decimal.Parse(this.ItemPrice.Text);
                        SellingPrice = Math.Round((decimal) price, 2);
                    }

                    var discount = decimal.Parse(textBlock1.Text);

                    DiscountAmount = Math.Round(SellingPrice*Math.Round((discount/100), 2), 2);

                    var price1 = decimal.Parse(this.ItemPrice.Text);

                    this.ItemDiscountAmount.Text = DiscountAmount.ToString();
                    this.ItemNetPrice.Text = Math.Round(price1 - DiscountAmount, 2).ToString();

                    this.TaxCombo.SelectedValuePath = "Id";
                    this.TaxCombo.SelectedValue = 9;
                    this.ItemVatRate.Text = "0.00";
                    this.ItemVatAmount.Text = "0.00";
                }
                else
                {
                    this.ItemDiscountRate.Text = String.Format("{0:N}",textBlock1.Text);
                    this.ItemDiscountAmount.Text =
                        (decimal.Parse(this.ItemPrice.Text)*
                         Math.Round((decimal.Parse(this.ItemDiscountRate.Text) / 100m), 2)).ToString();
                    this.ItemNetPrice.Text =
                        Math.Round(decimal.Parse(this.ItemPrice.Text) - decimal.Parse(this.ItemDiscountAmount.Text), 2).ToString();

                    ComputeAmount();

                    ComputeVatAmount();

                    if(textBlock.Text == "Variable Discount")
                    {
                        this.ItemDiscountAmount.IsEnabled = true;
                    }
                    else
                    {
                        this.ItemDiscountAmount.IsEnabled = false;
                    }
                }
            }
        }
        private void ItemDiscountAmount_Change(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (decimal.Parse(this.ItemDiscountAmount.Text) <= decimal.Parse(this.ItemPrice.Text))
                {
                    this.ItemDiscountAmount.Text = this.ItemDiscountAmount.Text == null
                        ? "0.00"
                        : String.Format("{0:N}",decimal.Parse(this.ItemDiscountAmount.Text));
                    this.ItemDiscountRate.Text = String.Format("{0:N}",
                        (decimal.Parse(this.ItemDiscountAmount.Text)/decimal.Parse(this.ItemPrice.Text))*100m);

                    this.ItemNetPrice.Text = String.Format("{0:N}",
                        decimal.Parse(this.ItemPrice.Text) - decimal.Parse(this.ItemDiscountAmount.Text));

                    ComputeAmount();
                    ComputeVatAmount();
                }
                else
                {
                    MessageDialog dialogue = new MessageDialog("Discount amount exceeds item price.", "Discount");
                    dialogue.ShowAsync();
                    this.ItemDiscountAmount.Text = "0.00";
                    this.ItemDiscountRate.Text = "0.00";
                }
            }
        }
        private void UnitCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var comboBoxItem =
                    comboBox.ItemContainerGenerator.ContainerFromItem(comboBox.SelectedItem) as ComboBoxItem;
                if (comboBoxItem == null)
                {
                    return;
                }
                var textBlock = FindVisualChildByName<TextBlock>(comboBoxItem, "UnitIndex");
            }
        }
        private void TaxCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var comboBoxItem = comboBox.ItemContainerGenerator.ContainerFromItem(comboBox.SelectedItem) as ComboBoxItem;
                if (comboBoxItem == null)
                {
                    return;
                }
                var textBlock = FindVisualChildByName<TextBlock>(comboBoxItem, "RateIndex");
            }
        }
        #endregion

        #region Control Events
        private async void cmdCloseSalesOrderLine_Click(object sender, RoutedEventArgs e)
        {
            service = new Pos13_app_serviceClient();

            this.ItemQuantity.Text = Math.Abs(decimal.Parse(this.ItemQuantity.Text)).ToString();

            if (decimal.Parse(this.ItemDiscountAmount.Text) <= decimal.Parse(this.ItemPrice.Text))
            {
                this.ItemDiscountAmount.Text = this.ItemDiscountAmount.Text == null
                    ? "0.00"
                    : String.Format("{0:N}", decimal.Parse(this.ItemDiscountAmount.Text));
                this.ItemDiscountRate.Text = String.Format("{0:N}",
                    (decimal.Parse(this.ItemDiscountAmount.Text) / decimal.Parse(this.ItemPrice.Text)) * 100m);

                this.ItemNetPrice.Text = String.Format("{0:N}",
                    decimal.Parse(this.ItemPrice.Text) - decimal.Parse(this.ItemDiscountAmount.Text));
            }
            else
            {
                MessageDialog dialogue = new MessageDialog("Discount amount exceeds item price.", "Discount");
                dialogue.ShowAsync();
                this.ItemDiscountAmount.Text = "0.00";
                this.ItemDiscountRate.Text = "0.00";
            }

            ComputeAmount();
            ComputeVatAmount();

            await service.UpdateInvoiceTotalAmountAsync(InvoiceId);

            TrnSalesOrderDetailItemDetail.IsOpen = false;
        }
        private void cmdSaveSalesOrderLine_Click(object sender, RoutedEventArgs e)
        {
            service = new Pos13_app_serviceClient();

            this.ItemQuantity.Text = Math.Abs(decimal.Parse(this.ItemQuantity.Text)).ToString();

            if (decimal.Parse(this.ItemDiscountAmount.Text) <= decimal.Parse(this.ItemPrice.Text))
            {
                this.ItemDiscountAmount.Text = this.ItemDiscountAmount.Text == null
                    ? "0.00"
                    : String.Format("{0:N}", decimal.Parse(this.ItemDiscountAmount.Text));
                this.ItemDiscountRate.Text = String.Format("{0:N}",
                    (decimal.Parse(this.ItemDiscountAmount.Text) / decimal.Parse(this.ItemPrice.Text)) * 100m);

                this.ItemNetPrice.Text = String.Format("{0:N}",
                    decimal.Parse(this.ItemPrice.Text) - decimal.Parse(this.ItemDiscountAmount.Text));
            }
            else
            {
                MessageDialog dialogue = new MessageDialog("Discount amount exceeds item price.", "Discount");
                dialogue.ShowAsync();
                this.ItemDiscountAmount.Text = "0.00";
                this.ItemDiscountRate.Text = "0.00";
            }

            ComputeAmount();
            ComputeVatAmount();

            SaveSalesOrderLine(SalesLineId);

            service.UpdateInvoiceTotalAmountAsync(InvoiceId);

            GetSalesOrderDetail();

            GetSalesOrderDetailItem(InvoiceId);

            TrnSalesOrderDetailItemDetail.IsOpen = false;
        }
        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;
            var strOut = b.Tag.ToString().Split('-');

            var SLId = int.Parse(strOut[0]);
            var ItemId = int.Parse(strOut[1]);

            SalesLineId = SLId;

            EditItemPopUp(SLId, ItemId);
        }
        private async void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            var b = e.OriginalSource as Button;

            var message = new MessageDialog("You have deleted " + b.Tag,"Sales Order Line");
            message.ShowAsync();

            service = new Pos13_app_serviceClient();

            await service.DeleteSalesLineAsync(int.Parse(b.Tag.ToString()));

            GetSalesOrderDetail();

            GetSalesOrderDetailItem(InvoiceId);

            await service.UpdateInvoiceTotalAmountAsync(InvoiceId);

        }
        private void cmdSalesOrderDetailSave_Click(object sender, RoutedEventArgs e)
        {
            if (InvoiceId > 0)
            {
                SaveSalesOrder();
            }
            TrnSalesOrderDetailSave.IsOpen = false;
        }
        private void cmdSalesOrderDetailClose_Click(object sender, RoutedEventArgs e)
        {
            TrnSalesOrderDetailSave.IsOpen = false;
        }
        private async void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            service = new Pos13_app_serviceClient();

            var data = await service.TrnSalesOrderDetailAsync(InvoiceId);

            if (InvoiceId != 0 && data.Count > 0)
            {
                if (TrnSalesOrderDetailSave.IsOpen == false)
                {
                    var dataCustomer = await service.CustomerComboAsync();
                    var dataTerm = await service.TermComboAsync();
                    var dataUser = await service.UserComboAsync();

                    this.CustomerCombo.ItemsSource = dataCustomer;
                    this.CustomerCombo.SelectedValuePath = "Id";

                    this.TermCombo.ItemsSource = dataTerm;
                    this.TermCombo.SelectedValuePath = "Id";

                    this.SalesWaiterCombo.ItemsSource = dataUser;
                    this.SalesWaiterCombo.SelectedValuePath = "Id";

                    this.CustomerCombo.SelectedValue = data[0].CustomerId;
                    this.TermCombo.SelectedValue = data[0].TermId;
                    this.SalesWaiterCombo.SelectedValue = data[0].SalesAgent;
                    this.RemarksDescription.Text = data[0].Remarks;
                    this.PaxDescription.Text = data[0].Pax.ToString();

                    TrnSalesOrderDetailSave.IsOpen = true;
                }
                else
                {
                    TrnSalesOrderDetailSave.IsOpen = false;
                }
            }
            else if (InvoiceId != 0 && data.Count == 0)
            {
                data = await service.TrnSalesOrderDetailZeroAsync(InvoiceId);

                if (TrnSalesOrderDetailSave.IsOpen == false)
                {
                    var dataCustomer = await service.CustomerComboAsync();
                    var dataTerm = await service.TermComboAsync();
                    var dataUser = await service.UserComboAsync();

                    this.CustomerCombo.ItemsSource = dataCustomer;
                    this.CustomerCombo.SelectedValuePath = "Id";

                    this.TermCombo.ItemsSource = dataTerm;
                    this.TermCombo.SelectedValuePath = "Id";

                    this.SalesWaiterCombo.ItemsSource = dataUser;
                    this.SalesWaiterCombo.SelectedValuePath = "Id";

                    this.CustomerCombo.SelectedValue = data[0].CustomerId;
                    this.TermCombo.SelectedValue = data[0].TermId;
                    this.SalesWaiterCombo.SelectedValue = data[0].SalesAgent;
                    this.RemarksDescription.Text = data[0].Remarks;
                    this.PaxDescription.Text = data[0].Pax.ToString();

                    TrnSalesOrderDetailSave.IsOpen = true;
                }
                else
                {
                    TrnSalesOrderDetailSave.IsOpen = false;
                }
            }
        }
        public void cmdClose_Click(object sender, RoutedEventArgs e)
        {
           this.Frame.GoBack();
        }

        #endregion

        #region Functions
        private async void GetMinItemGroupId()
        {
            ItemGroupId = await service.MinItemGroupIdAsync();
        }
        private async void SaveSalesOrder()
        {          
            var data = new Pos13_app_serviceClient();

            var CurrentDate = InvoiceDate.ToString().Substring(0, 8);

            var SLId = InvoiceId;
            var PId = 1;
            var SalesDate = InvoiceId == 0
                ? DateTime.Now.Date
                : DateTime.Parse(CurrentDate);
            var SalesNumber = InvoiceId == 0 ? "NA" : InvoiceNo.Text;
            var ManualInvoiceNumber = InvoiceId == 0 ? "NA" : InvoiceNo.Text;
            var Amount = InvoiceId == 0 ? 0 : decimal.Parse(this.Amount.Text.Replace("₱", ""));
            var TBId = TableId;

            CustomerCombo.SelectedValuePath = "Id";

            var CustomerId = InvoiceId == 0 ? 5451 : int.Parse(CustomerCombo.SelectedValue.ToString());
            var AccountId = 64;

            TermCombo.SelectedValuePath = "Id";

            var TermId = InvoiceId == 0 ? 7 : int.Parse(TermCombo.SelectedValue.ToString());
            var DiscountId = 3;
            var SeniorCitizenId = "";
            var SeniorCitizenName = "";
            var SeniorCitizenAge = 0;
            var Remarks = InvoiceId == 0 ? "NA" : RemarksDescription.Text;

            SalesWaiterCombo.SelectedValuePath = "Id";

            var SalesAgent = InvoiceId == 0 ? userId : int.Parse(SalesWaiterCombo.SelectedValue.ToString());
            var TerminalId = 1;
            var PreparedBy = userId;
            var CheckedBy = userId;
            var ApprovedBy = userId;
            var IsLocked = false;
            var IsCancelled = false;
            var PaidAmount = 0;
            var CreditAmount = 0;
            var DebitAmount = 0;
            var BalanceAmount = 0;
            var EntryUserId = userId;
            var EntryDateTime = DateTime.Now;
            var UpdateUserId =userId;
            var UpdateDateTime = DateTime.Now;
            var Pax = InvoiceId == 0 ? 1: int.Parse(PaxDescription.Text);

            if (TermId != null)
            {
                if (CustomerId != null)
                {
                    var Sales = new TrnSales()
                    {
                        Id = SLId,
                        PeriodId = PId,
                        SalesDate = SalesDate,
                        SalesNumber = SalesNumber,
                        ManualInvoiceNumber = ManualInvoiceNumber,
                        Amount = Amount,
                        TableId = TBId,
                        CustomerId = CustomerId,
                        AccountId = AccountId,
                        TermId = TermId,
                        DiscountId = DiscountId,
                        SeniorCitizenId = SeniorCitizenId,
                        SeniorCitizenName = SeniorCitizenName,
                        SeniorCitizenAge = SeniorCitizenAge,
                        Remarks = Remarks,
                        SalesAgent = SalesAgent,
                        TerminalId = TerminalId,
                        PreparedBy = PreparedBy,
                        CheckedBy = CheckedBy,
                        ApprovedBy = ApprovedBy,
                        IsLocked = IsLocked,
                        IsCancelled = IsCancelled,
                        PaidAmount = PaidAmount,
                        CreditAmount = CreditAmount,
                        DebitAmount = DebitAmount,
                        BalanceAmount = BalanceAmount,
                        EntryUserId = EntryUserId,
                        EntryDateTime = EntryDateTime,
                        UpdateUserId = UpdateUserId,
                        UpdateDateTime = UpdateDateTime,
                        Pax = Pax
                    };

                    await data.InserSalesOrderAsync(
                        Sales.Id,
                        Sales.PeriodId,
                        Sales.SalesDate,
                        Sales.SalesNumber,
                        Sales.ManualInvoiceNumber,
                        Sales.Amount,
                        Sales.TableId,
                        Sales.CustomerId,
                        Sales.AccountId,
                        Sales.TermId,
                        Sales.DiscountId,
                        Sales.SeniorCitizenId,
                        Sales.SeniorCitizenName,
                        Sales.SeniorCitizenAge,
                        Sales.Remarks,
                        Sales.SalesAgent,
                        Sales.TerminalId,
                        Sales.PreparedBy,
                        Sales.CheckedBy,
                        Sales.ApprovedBy,
                        Sales.IsLocked,
                        Sales.IsCancelled,
                        Sales.PaidAmount,
                        Sales.CreditAmount,
                        Sales.DebitAmount,
                        Sales.BalanceAmount,
                        Sales.EntryUserId,
                        Sales.EntryDateTime,
                        Sales.UpdateUserId,
                        Sales.UpdateDateTime,
                        Sales.Pax,
                        "Insert"
                        );
                }
            }
        }

        private async void SaveSalesOrderLine(int SalesLineId)
        {
            var data = new Pos13_app_serviceClient();

            var SalesId = InvoiceId;
            var ItemId = int.Parse(this.ItemCombo.SelectedValue.ToString());
            var UnitId = int.Parse(this.UnitCombo.SelectedValue.ToString());
            var Price = decimal.Parse(this.ItemPrice.Text);
            var DiscountId = int.Parse(this.DiscountCombo.SelectedValue.ToString());
            var DiscountRate = decimal.Parse(this.ItemDiscountRate.Text);
            var DiscountAmount = decimal.Parse(this.ItemDiscountAmount.Text);
            var NetPrice = decimal.Parse(this.ItemNetPrice.Text);
            var Quantity = decimal.Parse(this.ItemQuantity.Text);
            var Amount = decimal.Parse(this.ItemAmount.Text);
            var TaxId = int.Parse(this.TaxCombo.SelectedValue.ToString());
            var TaxRate = decimal.Parse(this.ItemVatRate.Text);
            var TaxAmount = decimal.Parse(this.ItemVatAmount.Text);
            var SalesAccountId = 159;
            var AssetAccountId = 74;
            var CostAccountId = 238;
            var TaxAccountId =87;
            var SalesLineTimeStamp = DateTime.Now;
            var UserId = 1;
            var Preparation = "NA";

            var SalesLine = new TrnSalesLine()
            {
                Id = SalesLineId,
                SalesId = SalesId,
                ItemId = ItemId,
                UnitId = UnitId,
                Price = Price,
                DiscountId = DiscountId,
                DiscountRate = DiscountRate,
                DiscountAmount = DiscountAmount,
                NetPrice = NetPrice,
                Quantity = Quantity,
                Amount = Amount,
                TaxId = TaxId,
                TaxRate = TaxRate,
                TaxAmount = TaxAmount,
                SalesAccountId = SalesAccountId,
                AssetAccountId = AssetAccountId,
                CostAccountId = CostAccountId,
                TaxAccountId = TaxAccountId,
                SalesLineTimeStamp = SalesLineTimeStamp,
                UserId=UserId,
                Preparation = Preparation
            };

            await data.InsertSalesOrderLineAsync(
               SalesLine.Id,
               SalesLine.ItemId,
               SalesLine.SalesId,
               SalesLine.UnitId,
               SalesLine.Price,
               SalesLine.DiscountId,
               SalesLine.DiscountRate,
               SalesLine.DiscountAmount,
               SalesLine.NetPrice,
               SalesLine.Quantity,
               SalesLine.Amount,
               SalesLine.TaxId,
               SalesLine.TaxRate,
               SalesLine.TaxAmount,
               SalesLine.SalesAccountId,
               SalesLine.AssetAccountId,
               SalesLine.CostAccountId,
               SalesLine.TaxAccountId,
               SalesLine.SalesLineTimeStamp,
               SalesLine.UserId,
               SalesLine.Preparation
                );
             
        }
        private async void GetSalesOrderDetail()
        {
            service = new Pos13_app_serviceClient();

            var data = await service.TrnSalesOrderDetailAsync(InvoiceId);

            if (InvoiceId == 0)
            {
                InvoiceId = await service.GetTableInvoiceIdAsync(DateTime.Now.Date, TableId, "Open");
            }

            if (InvoiceId != 0 && data.Count > 0)
            {
                this.InvoiceNo.Text = data[0].SalesNumber;
                this.Terminal.Text = data[0].Terminal;
                this.Prepared.Text = data[0].PreparedBy;
                this.TransactionDate.Text = data[0].TransactionDate;

                InvoiceDate = data[0].SalesDate;

                this.GrossAmount.Text = data[0].AmountDescription;
                this.LessAmount.Text = data[0].LessAmountDescription;
                this.Amount.Text = data[0].GrossAmountDescription;

                this.InvoiceAmount.Text = data[0].AmountDescription;

            }
            else if (InvoiceId != 0 && data.Count == 0)
            {
                data = await service.TrnSalesOrderDetailZeroAsync(InvoiceId);

                this.InvoiceNo.Text = data[0].SalesNumber;
                this.Terminal.Text = data[0].Terminal;
                this.Prepared.Text = data[0].PreparedBy;
                this.TransactionDate.Text = data[0].TransactionDate;

                InvoiceDate = data[0].SalesDate;

                this.GrossAmount.Text = data[0].GrossAmountDescription;
                this.LessAmount.Text = data[0].LessAmountDescription;
                this.Amount.Text = data[0].AmountDescription;

                this.InvoiceAmount.Text = data[0].AmountDescription;
            }
        }

        private async void GetSalesOrderDetailItem(int InvoiceId)
        {
            service = new Pos13_app_serviceClient();

            var data = await service.TrnSalesOrderDetailItemAsync(InvoiceId);

            if (data != null)
            {
                TrnSalesOrderDetailItemList.DataContext = data;
            }
        }
        public async void EditItemPopUp(int InvoiceLineId, int ItemId)
        {

            if (ItemId != 0)
            {
                service = new Pos13_app_serviceClient();
                if (TrnSalesOrderDetailItemDetail.IsOpen == false)
                {
                    var data = await service.ItemComboBoxAsync();
                    var dataDiscount = await service.DiscountComboAsync(DateTime.Now.Date, ItemId);
                    var dataUnit = await service.UnitComboAsync();
                    var dataTax = await service.TaxComboAsync();

                    this.ItemCombo.ItemsSource = data;
                    this.ItemCombo.SelectedValuePath = "Id";
                    //this.ItemCombo.DisplayMemberPath = "@ItemDescription";

                    this.DiscountCombo.ItemsSource = dataDiscount;
                    this.DiscountCombo.SelectedValuePath = "Id";

                    this.UnitCombo.ItemsSource = dataUnit;
                    this.UnitCombo.SelectedValuePath = "Id";

                    this.TaxCombo.ItemsSource = dataTax;
                    this.TaxCombo.SelectedValuePath = "Id";

                    TrnSalesOrderDetailItemDetail.IsOpen = true;

                    GetSalesOrderDetailItemDetail(InvoiceLineId, ItemId);
                }
                else
                {
                    TrnSalesOrderDetailItemDetail.IsOpen = false;
                }
            }         
        }
        private async void GetSalesOrderDetailItemDetail(int InvoiceLineId,int ItemId)
        {
            service = new Pos13_app_serviceClient();

            if (InvoiceLineId != 0)
            {
                var data = await service.TrnSalesTrnSalesOrderDetailItemDetailAsync(InvoiceLineId);

                this.ItemCombo.SelectedValue = data.ItemId;
                this.ItemQuantity.Text = String.Format("{0:N}", data.Quantity);
                this.UnitCombo.SelectedValue = data.UnitId;
                this.ItemPrice.Text = String.Format("{0:N}", data.Price);
                this.DiscountCombo.SelectedValue = data.DiscountId;
                this.ItemDiscountRate.Text = String.Format("{0:N}", data.DiscountRate);
                this.ItemDiscountAmount.Text = String.Format("{0:N}", data.DiscountAmount);
                this.ItemNetPrice.Text = String.Format("{0:N}", data.NetPrice);
                this.ItemAmount.Text = String.Format("{0:N}", data.Amount);
                this.TaxCombo.SelectedValue = data.TaxId;
                this.ItemVatRate.Text = String.Format("{0:N}", data.TaxRate);
                this.ItemVatAmount.Text = String.Format("{0:N}", data.TaxAmount);
            }
            else
            {
                var dataItem = await service.ItemComboBoxAsync();
                var ItemPrice = (from i in dataItem
                    where i.Id == ItemId
                    select i.Price).FirstOrDefault();
                var ItemTaxId = (from i in dataItem
                                 where i.Id == ItemId
                                 select i.OutTaxId).FirstOrDefault();
                var ItemTaxRate = (from i in dataItem
                                 where i.Id == ItemId
                                 select i.VatRate).FirstOrDefault();

                this.ItemCombo.SelectedValue = ItemId;
                this.ItemQuantity.Text = "1.00";
                this.UnitCombo.SelectedValue = 1;
                this.ItemPrice.Text = String.Format("{0:N}",ItemPrice);
                this.DiscountCombo.SelectedValue = 2;
                this.ItemDiscountRate.Text ="0,00";
                this.ItemDiscountAmount.Text ="0.00";
                this.ItemNetPrice.Text = String.Format("{0:N}", ItemPrice);
                this.ItemAmount.Text =String.Format("{0:N}",ItemPrice);
                this.TaxCombo.SelectedValue = ItemTaxId;
                this.ItemVatRate.Text = String.Format("{0:N}", ItemTaxRate);
                this.ItemVatAmount.Text = "0.00";

                ComputeVatAmount();      
            }

            var dataDiscount = await service.DiscountComboAsync(DateTime.Now.Date,ItemId);

            var discount = (from i in dataDiscount
                where i.Id == int.Parse(DiscountCombo.SelectedValue.ToString())
                select i.Discount).FirstOrDefault();

            if (discount == "Variable Discount")
            {
                this.ItemDiscountAmount.IsEnabled = true;
            }
            else
            {
                this.ItemDiscountAmount.IsEnabled = false;
            }
        }
        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, object e)
        {
            FillItem(ItemGroupId);
        }
        public async void FillItemGroup()
        {
            const int ItemGroupNumberOfButtons = 6;

            var MstItemGroup = await service.GetMstItemGroupAsync();

            int MstItemGroupCount = MstItemGroup.Count();

            int indexer = 0;
            int addOnIndexer = 0;

            ItemGroupPages = ((MstItemGroupCount - (MstItemGroupCount % ItemGroupNumberOfButtons)) /
                          ItemGroupNumberOfButtons) +
                         ((MstItemGroupCount % ItemGroupNumberOfButtons) > 0 ? 1 : 0);

            ItemGroupPage = ItemGroupPage < 1 ? 1 : ItemGroupPage;
            ItemGroupPage = ItemGroupPage > ItemGroupPages ? ItemGroupPages : ItemGroupPage;

            addOnIndexer = ItemGroupNumberOfButtons * (ItemGroupPage - 1);
            addOnIndexer = addOnIndexer == 0 ? 0 : addOnIndexer;

            for (int i = 1; i <= ItemGroupNumberOfButtons; i++)
            {
                var buttonName = string.Format("CmdItemGroup{0}", GetRightString((i + 100).ToString(), 2));
                var button = (Button)FindName(buttonName);
                if (button != null)
                {
                    var buttonContent = new TextBlock()
                    {
                        Text = (indexer + addOnIndexer) >= MstItemGroupCount
                            ? ""
                            : MstItemGroup[indexer + addOnIndexer].ItemGroup
                    };

                    buttonContent.TextWrapping = TextWrapping.Wrap;
                    buttonContent.TextAlignment = TextAlignment.Center;

                    button.Content = buttonContent;
                    button.FontSize = 12;
                }
                if (button != null)
                    button.Tag = (indexer + addOnIndexer) >= MstItemGroupCount
                        ? 0.ToString()
                        : MstItemGroup[indexer + addOnIndexer].Id.ToString();
                indexer++;
            }
        }

        public async void FillItem(int ItemGroupId)
        {
            const int ItemNumberOfButtons = 30;

            var MstItem = await service.GetMstItemAsync(ItemGroupId);

            int MstItemCount = MstItem.Count();

            int indexer = 0;
            int addOnIndexer = 0;

            ItemPages = ((MstItemCount - (MstItemCount % ItemNumberOfButtons)) / ItemNumberOfButtons) +
                         ((MstItemCount % ItemNumberOfButtons) > 0 ? 1 : 0);

            ItemPage = ItemPage < 1 ? 1 : ItemPage;
            ItemPage = ItemPage > (ItemPages < 1 ? 1 : ItemPages) ? ItemPages : ItemPage;

            addOnIndexer = ItemNumberOfButtons * (ItemPage - 1);
            addOnIndexer = addOnIndexer == 0 ? 0 : addOnIndexer;

            for (int i = 1; i <= ItemNumberOfButtons; i++)
            {
                var buttonName = string.Format("cmdItem{0}", GetRightString((i + 100).ToString(), 2));
                var button = (Button)FindName(buttonName);

                if (button != null)
                {
                    var buttonContent = new TextBlock()
                    {
                        Text = (indexer + addOnIndexer) >= MstItemCount
                            ? ""
                            : MstItem[indexer + addOnIndexer].Alias
                    };

                    buttonContent.TextWrapping = TextWrapping.Wrap;
                    buttonContent.TextAlignment = TextAlignment.Center;

                    button.Content = buttonContent;

                    button.Tag = (indexer + addOnIndexer) >= MstItemCount
                        ? ""
                        : MstItem[indexer + addOnIndexer].Id.ToString();

                    button.FontSize = 12;
                }

                indexer++;
            }
        }        public static string GetRightString(string param, int length)
        {
            string result = param.Substring(param.Length - length, length);
            return result;
        }
        private async void ComputeAmount()
        {
            this.ItemAmount.Text = String.Format("{0:N}",Math.Round(decimal.Parse(this.ItemNetPrice.Text) * decimal.Parse(this.ItemQuantity.Text), 2));

            service = new Pos13_app_serviceClient();

            var data = await service.TaxComboAsync();
            int taxId = 0;

            var taxSelectedValue = this.TaxCombo.SelectedValue;
            if (taxSelectedValue != null)
            {
                taxId = int.Parse(taxSelectedValue.ToString());
            }

            var code = (from i in data
                where i.Id == taxId
                select i.Code).FirstOrDefault();

            if (code == "EXCLUSIVE")
            {
                this.ItemAmount.Text =
                    (decimal.Parse(this.ItemAmount.Text) + decimal.Parse(this.ItemVatAmount.Text)).ToString();
            }
        }
        private async void ComputeVatAmount()
        {
            decimal VATRate = decimal.Parse(this.ItemVatRate.Text) / 100;

            if (this.TaxCombo != null)
            {
                service = new Pos13_app_serviceClient();

                var data = await service.TaxComboAsync();
                int taxId = 0;

                var taxSelectedValue = this.TaxCombo.SelectedValue;
                if (taxSelectedValue != null)
                {
                    taxId = int.Parse(taxSelectedValue.ToString());
                }
                var code = (from i in data
                            where i.Id == taxId
                            select i.Code).FirstOrDefault();

                if (code == "INCLUSIVE")
                {
                    this.ItemVatAmount.Text =
                        (Math.Round(decimal.Parse(this.ItemAmount.Text) / (1 + VATRate) *
                                    VATRate, 2)).ToString();
                }
                else
                {
                    this.ItemVatAmount.Text =
                        (Math.Round(decimal.Parse(this.ItemAmount.Text) +
                                    (decimal.Parse(this.ItemAmount.Text) * VATRate), 2))
                            .ToString();
                }
            }
        }
        private static T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                T result = FindVisualChildByName<T>(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }
        #endregion
    }
}
