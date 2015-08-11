using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using CommonLibrary;
using System.Web.UI.HtmlControls;

public partial class ProductMachines : PageBase
{
    DA da = new DA();

    protected void Page_Load(object sender, EventArgs e)
    {
        pgc.PageNumberChanged += new EventHandler(pgc_PageNumberChanged);
        CheckAccessibility();
        if (!IsPostBack)
        {
            BindRadioButton();
        }
    }
    //private List<ProductMachineItem> ProductMachineItems
    //{
    //    get
    //    {
    //        if (Session["ProductMachineItems"] == null)
    //        {
    //            Session["ProductMachineItems"] = new List<ProductMachineItem>();
    //        }
    //        return (List<ProductMachineItem>)Session["ProductMachineItems"];
    //    }
    //}
    private void pgc_PageNumberChanged(object sender, EventArgs e)
    {
        //CheckedSelectedItems();

        PopulateGrid();
    }
    //void CheckedSelectedItems()
    //{
    //    foreach (RepeaterItem item in rep.Items)
    //    {
    //        CheckBox chk = (CheckBox)item.FindControl("chk");
    //        HiddenField hidProductID = (HiddenField)item.FindControl("hidProductID");

    //        if (chk.Checked)
    //        {
    //            if (!pgc.SelectedIDs.Contains(int.Parse(hidProductID.Value)))
    //            {
    //                pgc.SelectedIDs.Add(int.Parse(hidProductID.Value));
    //                //ProductMachineItems.Add(prepareCartRecord(item));
    //            }
    //        }
    //        else
    //        {
    //            if (pgc.SelectedIDs.Contains(int.Parse(hidProductID.Value)))
    //            {
    //                pgc.SelectedIDs.Remove(int.Parse(hidProductID.Value));
    //                //ProductMachineItems.Remove(prepareCartRecord(item));
    //            }
    //        }
    //    }
    //}
    void PopulateGrid()
    {
        ListData res = new ListData(1, 1, 1);
        dsProduct.ProductDataTable tbl = da.Product_Search(txtSearchCode.Text, txtSearchName.Text, (int.Parse(rdRecon.SelectedValue) == 1 ? 0 : 1), DateTime.Now, (rdIsPromotion.SelectedValue == "1" ? true : false), out res, pgc.CurrentPage);
        pgc.Populate(res);

        byte[] ls = new byte[tbl.Rows.Count];
        rep.DataSource = ls;
        rep.DataBind();

        for (int i = 0; i <= ls.Length - 1; i++)
        {
            dsProduct.ProductRow drProduct = (dsProduct.ProductRow)tbl.Rows[i];
            RepeaterItem item = (RepeaterItem)rep.Items[i];
            HtmlTableRow tr = (HtmlTableRow)item.FindControl("trRow");
            tr.Attributes.Add("class", (i % 2 == 0) ? "gridData" : "gridDataAlt");

            HiddenField hidProductID = (HiddenField)item.FindControl("hidProductID");
            hidProductID.Value = drProduct.ID.ToString();

            CheckBox chk = (CheckBox)item.FindControl("chk");

            LinkButton lnkDetails = (LinkButton)item.FindControl("lnkDetails");
            lnkDetails.Text = drProduct.Name;
            lnkDetails.CommandArgument = drProduct.ID.ToString();
            
            Literal litCode = (Literal)item.FindControl("litCode");
            litCode.Text = drProduct.Code;

            Button btnAddToCart = (Button)item.FindControl("btnAddToCart");
            btnAddToCart.CommandArgument = i.ToString();

            int quantity = 1;
            string price = "0.00 (Unknown Tier)";

            dsPOProductItem _dsPOProductItem = new dsPOProductItem();
            dsPOProductItem.POProductItemRow drPOProductItem = da.POProductItem_GetPOQuantity(WebLib.LoggedInUser.DealerUserID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTime.Now);
            if (drPOProductItem != null && drPOProductItem.Quantity > 0)
                    quantity = drPOProductItem.Quantity + 1;

            dsProductPriceTier _dsProductPriceTier = new dsProductPriceTier();
            dsProductPriceTier.ProductPriceTierRow drProductPriceTier = da.ProductPriceTier_Get(drProduct.ID, quantity);
            if (drProductPriceTier != null)
            {
                switch (drProductPriceTier.TierName)
                {
                    case "Tier1":
                        //price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (Tier 1)";
                        price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (Normal)";
                        break;

                    case "Tier2":
                        //price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (Tier 2)";
                        price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (Normal)";
                        break;

                    case "Tier3":
                        //price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (Tier 3)";
                        price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (Normal)";
                        break;

                    case "NoTier":
                        //price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (No Tier)";
                        price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (Normal)";
                        break;
                }
            }

            Label lblPrice = (Label)item.FindControl("lblPrice");
            lblPrice.Text = price;
            
            TextBox txtPrice = (TextBox)item.FindControl("txtPrice");
            txtPrice.Text = "0.00";

            RadioButtonList rdOrderType = (RadioButtonList)item.FindControl("rdOrderType");
            rdOrderType.Attributes.Add("onClick", "javascript:CheckedOrderType('" + rdOrderType.ClientID + "','" + lblPrice.ClientID + "','" + txtPrice.ClientID + "');");

            //if (pgc.SelectedIDs.Contains(drProduct.ID))
            //{
            //    chk.Checked = true;
            //}
            //else
            //{
            //    chk.Checked = false;
            //}
        }    
    }


    protected void rep_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "PRODUCTDETAIL")
        {
            ProductMachineItem machineItem = prepareCartRecord(e.Item);
            HttpContext.Current.Session["productMachine_ProductID"] = e.CommandArgument.ToString();
            HttpContext.Current.Session["ReturnURL"] = Request.Url.ToString();

            //if (!ProductMachineItems.Contains(machineItem))
            //{
            //    ProductMachineItems.Add(machineItem);
            //}
            HttpContext.Current.Session["productMachine_Item"] = machineItem;

            showProductDetails();
        }
    }    
    
    private ProductMachineItem prepareCartRecord(RepeaterItem item)
    {    
        LinkButton lnkDetails = (LinkButton)item.FindControl("lnkDetails"); //Product ID
        int productID = int.Parse(lnkDetails.CommandArgument);

        TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
        Label lblPrice = (Label)item.FindControl("lblPrice");
        TextBox txtPrice = (TextBox)item.FindControl("txtPrice");
        RadioButtonList rdOrderType = (RadioButtonList)item.FindControl("rdOrderType");


        dsCart _dsCart = new dsCart();
        dsCart.CartRow drCart = _dsCart.Cart.CreateNewRow();

        drCart.DealerUserID = WebLib.LoggedInUser.DealerUserID;
        drCart.CartItemID = productID;
        drCart.ItemTypeEnum = (int)ItemType.Machine;

        if (rdOrderType.SelectedValue == "Others")
        {
            drCart.OrderTypeEnum = (int)OrderType.Others;

            if (Common.IsNum(txtPrice.Text))
                drCart.UnitPrice = decimal.Parse(txtPrice.Text);
            else
                drCart.UnitPrice = 0;            
        }
        else
        {
            drCart.OrderTypeEnum = (int)OrderType.Normal;

            string[] price = lblPrice.Text.Split(' ');
            drCart.UnitPrice = decimal.Parse(price[0]);
        }

        drCart.IsPromotion = (rdIsPromotion.SelectedValue == "1" ? true : false);
        drCart.IsProduct = true;
        drCart.IsNewProduct = hidRdRecon.Value == "1" ? false : true;

        if (Common.IsNum(txtQuantity.Text) && int.Parse(txtQuantity.Text) <= 9999)
            drCart.Quantity = int.Parse(txtQuantity.Text);
        else
            drCart.Quantity = 1;

        drCart.CreatedBy = WebLib.LoggedInUser.DealerUserName;
        drCart.CreatedDate = DateTime.Now;
        drCart.ModifiedBy = WebLib.LoggedInUser.DealerUserName;
        drCart.ModifiedDate = DateTime.Now;

        ProductMachineItem productMachineItem = new ProductMachineItem();
        productMachineItem.dataRowCart = drCart;
        productMachineItem.dataSetCart = _dsCart;
        productMachineItem.productID = productID;

        return productMachineItem;
    }


    private int checkSelectCartRecord(int dealerUserID, int productID)
    {
        int cartID = 0;
        dsCart.CartDataTable tblCart = da.Cart_CheckDataByDealerUserID_CartItemID_IsProduct(dealerUserID, productID, true);
        foreach (dsCart.CartRow drCart in tblCart)
        {
            dsCartOptionalItem.CartOptionalItemDataTable tblCartOptionalItem = da.CartOptionalItem_GetByCartID(drCart.ID);            
            if (tblCartOptionalItem.Rows.Count == 0)
                    return drCart.ID;
        }

        return cartID;
    }

    protected void btnAddToCart_Click(object sender, EventArgs e)
    {
        Button btnAddToCart = (Button)sender;
        RepeaterItem item = (RepeaterItem)rep.Items[int.Parse(btnAddToCart.CommandArgument)]; //Get Current Repeater Row Index

        TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
        if (txtQuantity.Text == "" || !Common.IsNum(txtQuantity.Text))
        {
            txtQuantity.Text = "1";
        }

        ProductMachineItem productMachineItem = prepareCartRecord(item);
        //foreach (ProductMachineItem productMachineItem in ProductMachineItems)
        //{
            int cartID = checkSelectCartRecord(WebLib.LoggedInUser.DealerUserID, productMachineItem.productID);

            if (cartID > 0)
            {
                dsCart.CartRow drCart = da.Cart_GetDataByID(cartID);
                drCart.DealerUserID = productMachineItem.dataRowCart.DealerUserID;
                drCart.CartItemID = productMachineItem.dataRowCart.CartItemID;
                drCart.ItemTypeEnum = productMachineItem.dataRowCart.ItemTypeEnum;
                
                drCart.OrderTypeEnum = productMachineItem.dataRowCart.OrderTypeEnum;
                drCart.UnitPrice = productMachineItem.dataRowCart.UnitPrice;

                drCart.IsProduct = productMachineItem.dataRowCart.IsProduct;
                drCart.IsNewProduct = productMachineItem.dataRowCart.IsNewProduct;
                drCart.IsPromotion = productMachineItem.dataRowCart.IsPromotion;

                drCart.Quantity = productMachineItem.dataRowCart.Quantity; //productMachineItem.dataRowCart.Quantity + int.Parse(txtQuantity.Text);

                drCart.ModifiedBy = productMachineItem.dataRowCart.ModifiedBy;
                drCart.ModifiedDate = productMachineItem.dataRowCart.ModifiedDate;

                da.Cart_Update(drCart);
            }
            else
            {
                productMachineItem.dataSetCart.Cart.Rows.Add(productMachineItem.dataRowCart);
                da.Cart_Update(productMachineItem.dataRowCart);
            }
        //}

            dsProduct.ProductRow drProduct = da.Product_Get(productMachineItem.productID);
            if (drProduct != null)
            {
                MasterBase.Message = drProduct.Name + " has been added to cart successfully.";
            }
        //Response.Redirect("MyCart.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        pgc.CurrentPage = 1;
        hidRdRecon.Value = rdRecon.SelectedValue;
        PopulateGrid();
        pnlListing.Visible = true;
    }

    private void BindRadioButton()
    {
        RadioButtonListExp rblExp = new RadioButtonListExp();
        rblExp.Bind(rdRecon, typeof(YesNo));
        rdRecon.SelectedIndex = 0;

        rblExp.Bind(rdIsPromotion, typeof(YesNo));
        rdIsPromotion.SelectedIndex = 0;
    }

    #region Product Details
    private void showProductDetails()
    {
        pnlMachineListing.Visible = false;
        pnlDetails.Visible = true;

        if (HttpContext.Current.Session["productMachine_ProductID"] != null)
        {
            dsProduct _dsProduct = new dsProduct();
            dsProduct.ProductRow drProduct = da.Product_Get(int.Parse(HttpContext.Current.Session["productMachine_ProductID"].ToString()));
            if (drProduct == null)
            {
                MasterBase.Message = WebLib.RecordNotFound;
            }
            else
            {
                litProductName.Text = drProduct.Name + " (" + drProduct.Code + ")";
                
                dsMachineItem.MachineItemDataTable tblMachineItem = da.MachineItem_Search(drProduct.ID, (int)ProductItemType.Machine, true);
                //foreach (dsMachineItem.MachineItemRow drMachineItem in tblMachineItem.Rows)
                //{
                //    litProductCode.Text = "(EDP Code: " + drMachineItem.Code + ")";
                //}

                repMachine.DataSource = new byte[tblMachineItem.Rows.Count];
                repMachine.DataBind();

                for (int i = 0; i < repMachine.Items.Count; i++)
                {
                    RepeaterItem item = (RepeaterItem)repMachine.Items[i];
                    dsMachineItem.MachineItemRow drMachine = (dsMachineItem.MachineItemRow)tblMachineItem.Rows[i];

                    Literal litMachineName = (Literal)item.FindControl("litMachineName");
                    litMachineName.Text = "(EDP Code: " + drMachine.Code + ") " + drMachine.Name;
                }
                
                byte[] ls;

                //Service
                tblMachineItem = da.MachineItem_Search(drProduct.ID, (int)ProductItemType.Service, true);
                if (tblMachineItem.Rows.Count == 0)
                {
                    pnlStandardService.Visible = false;
                }
                else
                {
                    ls = new byte[tblMachineItem.Rows.Count];
                    repService.DataSource = ls;
                    repService.DataBind();

                    for (int i = 0; i < ls.Length; i++)
                    {
                        dsMachineItem.MachineItemRow drMachineItem = (dsMachineItem.MachineItemRow)tblMachineItem.Rows[i];
                        RepeaterItem item = repService.Items[i];

                        Literal litProductItemTypeConfig = (Literal)item.FindControl("litProductItemTypeService");
                        litProductItemTypeConfig.Text = "(EDP Code: " + drMachineItem.Code + ") " + drMachineItem.Name;
                    }
                }


                //Config
                tblMachineItem = da.MachineItem_Search(drProduct.ID, (int)ProductItemType.Config, true);
                if (tblMachineItem.Rows.Count == 0)
                {
                    pnlStandardConfig.Visible = false;
                }
                else
                {
                    ls = new byte[tblMachineItem.Rows.Count];
                    repConfig.DataSource = ls;
                    repConfig.DataBind();

                    for (int i = 0; i < ls.Length; i++)
                    {
                        dsMachineItem.MachineItemRow drMachineItem = (dsMachineItem.MachineItemRow)tblMachineItem.Rows[i];
                        RepeaterItem item = repConfig.Items[i];

                        Literal litProductItemTypeConfig = (Literal)item.FindControl("litProductItemTypeConfig");
                        litProductItemTypeConfig.Text = "(EDP Code: " + drMachineItem.Code + ") " + drMachineItem.Name;
                    }
                }

                //Others
                tblMachineItem = da.MachineItem_Search(drProduct.ID, (int)ProductItemType.OthersPromotionItems, true);
                if (tblMachineItem.Rows.Count == 0)
                {
                    pnlOthers.Visible = false;
                }
                else
                {
                    pnlOthers.Visible = true;

                    ls = new byte[tblMachineItem.Rows.Count];
                    repOthers.DataSource = ls;
                    repOthers.DataBind();

                    for (int i = 0; i < ls.Length; i++)
                    {
                        dsMachineItem.MachineItemRow drMachineItem = (dsMachineItem.MachineItemRow)tblMachineItem.Rows[i];
                        RepeaterItem item = repOthers.Items[i];

                        Literal litProductItemTypeOthers = (Literal)item.FindControl("litProductItemTypeOthers");
                        litProductItemTypeOthers.Text = "(EDP Code: " + drMachineItem.Code + ") " + drMachineItem.Name;
                    }
                }

                //Optional
                tblMachineItem = da.MachineItem_Search(drProduct.ID, (int)ProductItemType.Optional, true);
                if (tblMachineItem.Rows.Count == 0)
                {
                    pnlOptional.Visible = false;
                }
                else
                {
                    ls = new byte[tblMachineItem.Rows.Count];

                    chkProductItemTypeOptional.Items.Clear();
                    for (int i = 0; i < ls.Length; i++)
                    {
                        dsMachineItem.MachineItemRow drMachineItem = (dsMachineItem.MachineItemRow)tblMachineItem.Rows[i];
                        chkProductItemTypeOptional.Items.Add(new ListItem("(EDP Code: " + drMachineItem.Code + ") " + drMachineItem.Name, drMachineItem.ID.ToString()));
                    }

                    if (tblMachineItem.Rows.Count == 0)
                        pnlDetailsOptional.Visible = false;
                    else
                    {
                        pnlDetailsOptional.Visible = true;

                        repDetailsOptional.DataSource = new byte[tblMachineItem.Rows.Count];
                        repDetailsOptional.DataBind();

                        for (int i = 0; i < ls.Length; i++)
                        {
                            dsMachineItem.MachineItemRow drMachineItem = (dsMachineItem.MachineItemRow)tblMachineItem.Rows[i];
                            RepeaterItem itemOpt = (RepeaterItem)repDetailsOptional.Items[i];

                            HiddenField hidDetailsOptionalItemID = (HiddenField)itemOpt.FindControl("hidDetailsOptionalItemID");
                            hidDetailsOptionalItemID.Value = drMachineItem.ID.ToString();

                            Literal litDetailsEDPCode = (Literal)itemOpt.FindControl("litDetailsEDPCode");
                            litDetailsEDPCode.Text = drMachineItem.Code;

                            Literal litDetailsName = (Literal)itemOpt.FindControl("litDetailsName");
                            litDetailsName.Text = drMachineItem.Name;

                            Literal litDetailsUnitPrice = (Literal)itemOpt.FindControl("litDetailsUnitPrice");
                            litDetailsUnitPrice.Text = drMachineItem.UnitPrice.ToString();

                            TextBox txtDetailsQuantity = (TextBox)itemOpt.FindControl("txtDetailsQuantity");

                            CheckBox chk = (CheckBox)itemOpt.FindControl("chk");
                            chk.Attributes.Add("onClick", "javascript:CheckOptionalItemQuantity(this.id, '" + txtDetailsQuantity.ClientID + "');");

                            //foreach (ProductMachineItem machineItems in ProductMachineItems)
                            //{                            
                            //    if (machineItems.productID == drProduct.ID)
                            //    {
                            //        if (machineItems.SelectedOptionalItemsIDs.ContainsKey(drProduct.ID.ToString() + hidDetailsOptionalItemID.Value))
                            //        {
                            //            chk.Checked = true;
                            //            int intQuantity = 0;
                            //            machineItems.SelectedOptionalItemsIDs.TryGetValue(drProduct.ID.ToString() + hidDetailsOptionalItemID.Value, out intQuantity);
                            //            if (intQuantity > 0)
                            //            {
                            //                txtDetailsQuantity.Text = intQuantity.ToString();
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }
            }
        }
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        //foreach (RepeaterItem item in repDetailsOptional.Items)
        //{
        //    HiddenField hidDetailsOptionalItemID = (HiddenField)item.FindControl("hidDetailsOptionalItemID");

        //    TextBox txtDetailsQuantity = (TextBox)item.FindControl("txtDetailsQuantity");
        //    CheckBox chk = (CheckBox)item.FindControl("chk");
            
        //    if (chk.Checked)
        //    {
        //        if (txtDetailsQuantity.Text == "" || !Common.IsNum(txtDetailsQuantity.Text))
        //            txtDetailsQuantity.Text = "1";

        //        foreach (ProductMachineItem machineItems in ProductMachineItems)
        //        {
        //            ProductMachineItem currentProductMachineItem = (ProductMachineItem)HttpContext.Current.Session["productMachine_Item"];
        //            if (machineItems.productID == currentProductMachineItem.productID)
        //            {
        //                if (!machineItems.SelectedOptionalItemsIDs.ContainsKey(currentProductMachineItem.productID.ToString() + hidDetailsOptionalItemID.Value))
        //                {
        //                    machineItems.SelectedOptionalItemsIDs.Add(currentProductMachineItem.productID.ToString() + hidDetailsOptionalItemID.Value, int.Parse(txtDetailsQuantity.Text));
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (ProductMachineItem machineItems in ProductMachineItems)
        //        {
        //            ProductMachineItem currentProductMachineItem = (ProductMachineItem)HttpContext.Current.Session["productMachine_Item"];
        //            if (machineItems.productID == currentProductMachineItem.productID)
        //            {
        //                if (machineItems.SelectedOptionalItemsIDs.ContainsKey(currentProductMachineItem.productID.ToString() + hidDetailsOptionalItemID.Value))
        //                {
        //                    machineItems.SelectedOptionalItemsIDs.Remove(currentProductMachineItem.productID.ToString() + hidDetailsOptionalItemID.Value);
        //                }
        //            }
        //        }
        //    }
        //}

        pnlMachineListing.Visible = true;
        pnlDetails.Visible = false;
    }

    private int checkSelectCartRecordDetails(int dealerUserID, int productID)
    {
        int cartID = 0;

        dsCart.CartDataTable tblCart = da.Cart_CheckDataByDealerUserID_CartItemID_IsProduct(dealerUserID, productID, true);
        foreach (dsCart.CartRow drCart in tblCart)
        {
            int selectedCount = 0;

            dsCartOptionalItem.CartOptionalItemDataTable tblCartOptionalItem = da.CartOptionalItem_GetByCartID(drCart.ID);
            for (int i = 0; i < chkProductItemTypeOptional.Items.Count; i++)
                if (chkProductItemTypeOptional.Items[i].Selected)
                    selectedCount += 1;

            if (selectedCount == tblCartOptionalItem.Rows.Count)
            {
                int totalMatch = 0;
                for (int i = 0; i < chkProductItemTypeOptional.Items.Count; i++)
                {
                    if (chkProductItemTypeOptional.Items[i].Selected)
                    {
                        foreach (dsCartOptionalItem.CartOptionalItemRow drCartOptionalItem in tblCartOptionalItem)
                        {
                            if (drCartOptionalItem.ItemID == int.Parse(chkProductItemTypeOptional.Items[i].Value))
                            {
                                totalMatch += 1;
                                break;
                            }
                        }
                    }
                }
                if (totalMatch == selectedCount)
                    return drCart.ID;
            }
        }

        return cartID;
    }

    protected void btnAddToCartDetails_Click(object sender, EventArgs e)
    {

        ProductMachineItem productMachineItem = (ProductMachineItem)HttpContext.Current.Session["productMachine_Item"];
        int cartID = checkSelectCartRecordDetails(WebLib.LoggedInUser.DealerUserID, productMachineItem.productID);
        
        if (cartID > 0)
        {
            dsCart.CartRow drCart = da.Cart_GetDataByID(cartID);
            drCart.DealerUserID = productMachineItem.dataRowCart.DealerUserID;
            drCart.CartItemID = productMachineItem.dataRowCart.CartItemID;
            drCart.ItemTypeEnum = productMachineItem.dataRowCart.ItemTypeEnum;

            drCart.OrderTypeEnum = productMachineItem.dataRowCart.OrderTypeEnum;
            drCart.UnitPrice = productMachineItem.dataRowCart.UnitPrice;

            drCart.IsProduct = productMachineItem.dataRowCart.IsProduct;
            drCart.IsNewProduct = productMachineItem.dataRowCart.IsNewProduct;
            drCart.IsPromotion = productMachineItem.dataRowCart.IsPromotion;
            drCart.Quantity = productMachineItem.dataRowCart.Quantity;

            drCart.ModifiedBy = productMachineItem.dataRowCart.ModifiedBy;
            drCart.ModifiedDate = productMachineItem.dataRowCart.ModifiedDate;

            da.Cart_Update(drCart);
        }
        else
        {
            //Insert Cart Record
            productMachineItem.dataSetCart.Cart.Rows.Add(productMachineItem.dataRowCart);
            da.Cart_Update(productMachineItem.dataRowCart);
            int maxCartID = productMachineItem.dataRowCart.ID;

            dsCartOptionalItem _dsCartOptionalItem = new dsCartOptionalItem();
            dsCartOptionalItem.CartOptionalItemRow drCartOptionalItem;

            foreach (RepeaterItem item in repDetailsOptional.Items)
            {
                HiddenField hidDetailsOptionalItemID = (HiddenField)item.FindControl("hidDetailsOptionalItemID");
                drCartOptionalItem = da.CartOptionalItem_Get(maxCartID, int.Parse(hidDetailsOptionalItemID.Value));

                CheckBox chk = (CheckBox)item.FindControl("chk");
                if (chk.Checked)
                {
                    if (drCartOptionalItem == null)
                    {
                        dsMachineItem.MachineItemRow drItem = da.MachineItem_Get(int.Parse(hidDetailsOptionalItemID.Value));

                        TextBox txtDetailsQuantity = (TextBox)item.FindControl("txtDetailsQuantity");

                        drCartOptionalItem = _dsCartOptionalItem.CartOptionalItem.NewCartOptionalItemRow();
                        drCartOptionalItem.ItemID = drItem.ID;// int.Parse(hidDetailsOptionalItemID.Value);
                        drCartOptionalItem.CartID = maxCartID;
                        drCartOptionalItem.Quantity = int.Parse(txtDetailsQuantity.Text);
                        drCartOptionalItem.UnitPrice = drItem.UnitPrice;
                        drCartOptionalItem.CreatedBy = WebLib.LoggedInUser.DealerUserName;
                        drCartOptionalItem.CreatedDate = DateTime.Now;
                        drCartOptionalItem.ModifiedBy = WebLib.LoggedInUser.DealerUserName;
                        drCartOptionalItem.ModifiedDate = DateTime.Now;

                        _dsCartOptionalItem.CartOptionalItem.Rows.Add(drCartOptionalItem);
                        da.CartOptionalItem_Update(drCartOptionalItem);                        
                    }
                }
                else
                {
                    if (drCartOptionalItem != null)
                    {
                        drCartOptionalItem.Delete();
                        da.CartOptionalItem_Update(drCartOptionalItem);
                    }
                }
            }
        }

        dsProduct.ProductRow drProduct = da.Product_Get(productMachineItem.productID);
        if (drProduct != null)
        {
            MasterBase.Message = drProduct.Name + " has been added to cart successfully.";
        }

        pnlDetails.Visible = false;
        pnlMachineListing.Visible = true;
    }
    #endregion
}
