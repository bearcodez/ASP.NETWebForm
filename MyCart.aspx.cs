using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using CommonLibrary;
using System.Text;
using System.Web.UI.HtmlControls;

public partial class MyCart : PageBase
{
    DA da = new DA();
    int totalQuantityMachine = 0;
    decimal totalPriceMachine = 0;

    int totalQuantitySparePartsConsumables = 0;
    decimal totalPriceSparePartsConsumables = 0;
    public enum CartType
    {
        Machine,
        SparePartConsumable
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckAccessibility();
        if (!IsPostBack)
        {
            DisplayCarts();
        }
    }

    private void DisplayCarts()
    {
        if (da.Cart_GetItemCount(WebLib.LoggedInUser.DealerUserID) <= 0)
        {
            pnlNoRecord.Visible = true;
            pnlListing.Visible = false;
            pnlMachine.Visible = false;
            pnlSparePartsConsumables.Visible = false;
        }
        else
        {
            pnlNoRecord.Visible = false;
            pnlListing.Visible = true;
            pnlMachine.Visible = false;
            pnlSparePartsConsumables.Visible = false;

            if (da.Cart_GetMachineItemCount(WebLib.LoggedInUser.DealerUserID) > 0 || da.Cart_GetItemCount(WebLib.LoggedInUser.DealerUserID, (short)ItemType.Accessory) > 0)
            {
                pnlMachine.Visible = true;
                BindDataMachine();
            }

            if (da.Cart_GetSparePartsConsumableItemCount(WebLib.LoggedInUser.DealerUserID) > 0)
            {
                pnlSparePartsConsumables.Visible = true;
                BindDataSparePartsConsumables();
            }
            litGrandTotalQuantity.Text = (totalQuantityMachine + totalQuantitySparePartsConsumables).ToString();
            litGrandTotalPrice.Text = "RM " + (totalPriceMachine + totalPriceSparePartsConsumables).ToString(WebLib.CurrencyFormat);
        }
    }

    private void BindDataMachine()
    {
        dsCart.ViewProductCartDataTable tblCart = da.Cart_DisplayMachine(WebLib.LoggedInUser.DealerUserID, true);

        byte[] ls = new byte[tblCart.Rows.Count];
        repMachine.DataSource = ls;
        repMachine.DataBind();

        int totalQuantityMachineItem = da.Cart_TotalMachine(WebLib.LoggedInUser.DealerUserID);
        for (int i = 0; i <= ls.Length - 1; i++)
        {
            dsCart.ViewProductCartRow drProductCart = (dsCart.ViewProductCartRow)tblCart.Rows[i];
            RepeaterItem item = (RepeaterItem)repMachine.Items[i];

            totalQuantityMachine += drProductCart.Quantity;
        }

        for (int i = 0; i <= ls.Length - 1; i++)
        {
            dsCart.ViewProductCartRow drProductCart = (dsCart.ViewProductCartRow)tblCart.Rows[i];
            RepeaterItem item = (RepeaterItem)repMachine.Items[i];
            HtmlTableRow tr = (HtmlTableRow)item.FindControl("trRow");
            tr.Attributes.Add("class", (i % 2 == 0) ? "GridRow2" : "GridRowAlt2");

            Literal litNo = (Literal)item.FindControl("litNo");
            litNo.Text = (i + 1).ToString() + ".";

            Literal litEDPCode = (Literal)item.FindControl("litEDPCode");
            litEDPCode.Text = drProductCart.Code;

            Literal litName = (Literal)item.FindControl("litName");
            litName.Text = drProductCart.Name +(drProductCart.IsPromotion ? " <b>[Promotion]</b>" : "");

            Literal litUnitPrice = (Literal)item.FindControl("litUnitPrice");
            litUnitPrice.Text = (drProductCart.UnitPrice).ToString(WebLib.CurrencyFormat);

            TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
            txtQuantity.Text = drProductCart.Quantity.ToString();

            LinkButton lnkUpdateCart = (LinkButton)item.FindControl("lnkUpdateCart");
            lnkUpdateCart.CommandArgument = drProductCart.ID.ToString();

            LinkButton lnkRemove = (LinkButton)item.FindControl("lnkRemove");
            lnkRemove.CommandArgument = drProductCart.ID.ToString();

            Repeater repService = (Repeater)item.FindControl("repService");

            decimal totalOptionalItemPrice = 0;

            if (drProductCart.ItemTypeEnum == (short)ItemType.Accessory)
            {
                Literal litTotalPrice = (Literal)item.FindControl("litTotalPrice");
                litTotalPrice.Text = ((drProductCart.UnitPrice * drProductCart.Quantity)).ToString(WebLib.CurrencyFormat);
                totalPriceMachine += Decimal.Round((drProductCart.UnitPrice * drProductCart.Quantity), 2);
            }
            else
            {
                string tempOptionals = "";

                #region Machine Item
                Repeater repMachineItem = (Repeater)item.FindControl("repMachineItem");
                dsMachineItem.MachineItemDataTable tblItem = da.MachineItem_Search(drProductCart.CartItemID, (int)ProductItemType.Machine, true);
                if (tblItem.Rows.Count > 0)
                {
                    repMachineItem.DataSource = new byte[tblItem.Rows.Count];
                    repMachineItem.DataBind();

                    tr = (HtmlTableRow)repMachineItem.Controls[0].Controls[0].FindControl("trRow");
                    tr.Attributes.Add("class", (i % 2 == 0) ? "GridRow2" : "GridRowAlt2");
                    int machineCount = 0;
                    //tempOptionals += "<br /><b><u>Service</u></b>";
                    foreach (dsMachineItem.MachineItemRow drItem in tblItem.Rows)
                    {
                        tr = (HtmlTableRow)repMachineItem.Items[machineCount].FindControl("trRow");
                        tr.Attributes.Add("class", (i % 2 == 0) ? "GridRow2" : "GridRowAlt2");

                        Literal litMachineEDPCode = (Literal)repMachineItem.Items[machineCount].FindControl("litMachineEDPCode");
                        litMachineEDPCode.Text = drItem.Code;

                        Literal litMachineName = (Literal)repMachineItem.Items[machineCount].FindControl("litMachineName");
                        litMachineName.Text = drItem.Name;

                        //tempOptionals += drItem.Name + "<br />";
                        machineCount++;
                    }
                }
                #endregion

                tblItem = da.MachineItem_Search(drProductCart.CartItemID, (int)ProductItemType.Service, true);
                if (tblItem.Rows.Count > 0)
                {
                    repService.DataSource = new byte[tblItem.Rows.Count];
                    repService.DataBind();

                    tr = (HtmlTableRow)repService.Controls[0].Controls[0].FindControl("trRow");
                    tr.Attributes.Add("class", (i % 2 == 0) ? "GridRow2" : "GridRowAlt2");
                    int serviceCount = 0;
                    //tempOptionals += "<br /><b><u>Service</u></b>";
                    foreach (dsMachineItem.MachineItemRow drItem in tblItem.Rows)
                    {
                        tr = (HtmlTableRow)repService.Items[serviceCount].FindControl("trRow");
                        tr.Attributes.Add("class", (i % 2 == 0) ? "GridRow2" : "GridRowAlt2");

                        Literal litServiceEDPCode = (Literal)repService.Items[serviceCount].FindControl("litServiceEDPCode");
                        litServiceEDPCode.Text = drItem.Code;

                        Literal litServiceName = (Literal)repService.Items[serviceCount].FindControl("litServiceName");
                        litServiceName.Text = drItem.Name;

                        //tempOptionals += drItem.Name + "<br />";
                        serviceCount++;
                    }
                }
                else
                    repService.Visible = false;

                Repeater repConfig = (Repeater)item.FindControl("repConfig");

                tblItem = da.MachineItem_Search(drProductCart.CartItemID, (int)ProductItemType.Config, true);
                if (tblItem.Rows.Count > 0)
                {
                    repConfig.DataSource = new byte[tblItem.Rows.Count];
                    repConfig.DataBind();

                    int configCount = 0;

                    tr = (HtmlTableRow)repConfig.Controls[0].Controls[0].FindControl("trRow");
                    tr.Attributes.Add("class", (i % 2 == 0) ? "GridRow2" : "GridRowAlt2");
                    //tempOptionals += "<br /><b><u>Config</u></b>";
                    foreach (dsMachineItem.MachineItemRow drItem in tblItem.Rows)
                    {
                        tr = (HtmlTableRow)repConfig.Items[configCount].FindControl("trRow");
                        tr.Attributes.Add("class", (i % 2 == 0) ? "GridRow2" : "GridRowAlt2");

                        Literal litConfigEDPCode = (Literal)repConfig.Items[configCount].FindControl("litConfigEDPCode");
                        litConfigEDPCode.Text = drItem.Code;

                        Literal litConfigName = (Literal)repConfig.Items[configCount].FindControl("litConfigName");
                        litConfigName.Text = drItem.Name;

                        //tempOptionals += "<br />" + drItem.Name;
                        configCount++;
                    }
                }

                Repeater repOthers = (Repeater)item.FindControl("repOthers");

                tblItem = da.MachineItem_Search(drProductCart.CartItemID, (int)ProductItemType.OthersPromotionItems, true);
                if (tblItem.Rows.Count > 0)
                {
                    repOthers.DataSource = new byte[tblItem.Rows.Count];
                    repOthers.DataBind();

                    int othersCount = 0;
                    tr = (HtmlTableRow)repOthers.Controls[0].Controls[0].FindControl("trRow");
                    tr.Attributes.Add("class", (i % 2 == 0) ? "GridRow2" : "GridRowAlt2");
                    //tempOptionals += "<br /><b><u>Config</u></b>";
                    foreach (dsMachineItem.MachineItemRow drItem in tblItem.Rows)
                    {
                        tr = (HtmlTableRow)repOthers.Items[othersCount].FindControl("trRow");
                        tr.Attributes.Add("class", (i % 2 == 0) ? "GridRow2" : "GridRowAlt2");
                        Literal litOthersEDPCode = (Literal)repOthers.Items[othersCount].FindControl("litOthersEDPCode");
                        litOthersEDPCode.Text = drItem.Code;

                        Literal litOthersName = (Literal)repOthers.Items[othersCount].FindControl("litOthersName");
                        litOthersName.Text = drItem.Name;

                        othersCount++;
                    }
                }

                Repeater repOptional = (Repeater)item.FindControl("repOptional");

                dsCartOptionalItem.ViewCartOptionalItemDataTable tblViewCartOptionalItem = da.CartOptionalItem_Display(drProductCart.ID);
                if (tblViewCartOptionalItem.Rows.Count > 0)
                {
                    repOptional.DataSource = new byte[tblViewCartOptionalItem.Rows.Count];
                    repOptional.DataBind();

                    tr = (HtmlTableRow)repOptional.Controls[0].Controls[0].FindControl("trRow");
                    tr.Attributes.Add("class", (i % 2 == 0) ? "GridRow2" : "GridRowAlt2");
                    //litEDPCode.Text += "<br />";
                    //tempOptionals += "<br /><b><u>Optional</u></b>";
                }

               
                int optCount = 0;
                foreach (dsCartOptionalItem.ViewCartOptionalItemRow row in tblViewCartOptionalItem)
                {
                    tr = (HtmlTableRow)repOptional.Items[optCount].FindControl("trRow");
                    tr.Attributes.Add("class", (i % 2 == 0) ? "GridRow2" : "GridRowAlt2");

                    Literal litOptionalEDPCode = (Literal)repOptional.Items[optCount].FindControl("litOptionalEDPCode");
                    litOptionalEDPCode.Text = row.Code;

                    Literal litOptionalName = (Literal)repOptional.Items[optCount].FindControl("litOptionalName");
                    litOptionalName.Text = row.Name;

                    Literal litOptionalUnitPrice = (Literal)repOptional.Items[optCount].FindControl("litOptionalUnitPrice");
                    litOptionalUnitPrice.Text = row.Price.ToString(WebLib.CurrencyFormat);

                    TextBox txtOptionalQuantity = (TextBox)repOptional.Items[optCount].FindControl("txtOptionalQuantity");
                    txtOptionalQuantity.Text = row.Quantity.ToString();

                    HiddenField hidOptionalItemID = (HiddenField)repOptional.Items[optCount].FindControl("hidOptionalItemID");
                    hidOptionalItemID.Value = row.ID.ToString();

                    //litEDPCode.Text += "<br />" + row.Code;
                    //tempOptionals += "<br />" + row.Name;
                    totalOptionalItemPrice += row.UnitPrice * row.Quantity;

                    optCount++;
                }

                //litName.Text += "<br /><br />";

                //if (tempOptionals != "")
                //    litEDPCode.Text += "<br />";
                
                HiddenField hidIsNoTier = (HiddenField)item.FindControl("hidIsNoTier");

                if (drProductCart.OrderTypeEnum == (short)OrderType.Normal)
                {
                    int quantity = 0;
                    dsPOProductItem.POProductItemRow drPOProductItem = da.POProductItem_GetPOQuantity(WebLib.LoggedInUser.DealerUserID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTime.Now);
                    if (drPOProductItem != null && drPOProductItem.Quantity > 0)
                        quantity = drPOProductItem.Quantity;// +1;

                    string price = "";
                    dsProductPriceTier _dsProductPriceTier = new dsProductPriceTier();
                    dsProductPriceTier.ProductPriceTierRow drProductPriceTier = da.ProductPriceTier_Get(drProductCart.CartItemID, (quantity + totalQuantityMachineItem));
                    if (drProductPriceTier != null)
                    {
                        switch (drProductPriceTier.TierName)
                        {
                            case "Tier1":
                                price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (Normal)";
                                break;

                            case "Tier2":
                                price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (Normal)";
                                break;

                            case "Tier3":
                                price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (Normal)";
                                break;

                            case "NoTier":
                                price = drProductPriceTier.Price.ToString(WebLib.CurrencyFormat) + " (Normal)";
                                break;
                        }

                        litUnitPrice.Text = price;

                        Literal litTotalPrice = (Literal)item.FindControl("litTotalPrice");
                        litTotalPrice.Text = (((drProductPriceTier != null ? drProductPriceTier.Price : drProductCart.UnitPrice) * drProductCart.Quantity) + totalOptionalItemPrice).ToString(WebLib.CurrencyFormat);
                        totalPriceMachine += Decimal.Round((((drProductPriceTier != null ? drProductPriceTier.Price : drProductCart.UnitPrice) * drProductCart.Quantity) + totalOptionalItemPrice), 2);
                    }

                }
                else
                {
                    litUnitPrice.Text = (drProductCart.UnitPrice).ToString(WebLib.CurrencyFormat);
                    Literal litTotalPrice = (Literal)item.FindControl("litTotalPrice");
                    litTotalPrice.Text = ((drProductCart.UnitPrice * drProductCart.Quantity) + totalOptionalItemPrice).ToString(WebLib.CurrencyFormat);
                    totalPriceMachine += Decimal.Round((drProductCart.UnitPrice * drProductCart.Quantity) + totalOptionalItemPrice, 2);
                }

                
            }


            
        }

        litMachineTotalQuantity.Text = totalQuantityMachine.ToString();
        litMachineTotalPrice.Text = "RM " + totalPriceMachine.ToString(WebLib.CurrencyFormat);
    }

    private void BindDataSparePartsConsumables()
    {
        dsCart.ViewItemCartDataTable tblCart = da.Cart_DisplaySparePartsConsumables(WebLib.LoggedInUser.DealerUserID);

        byte[] ls = new byte[tblCart.Rows.Count];
        repSparePartsConsumables.DataSource = ls;
        repSparePartsConsumables.DataBind();

        for (int i = 0; i <= ls.Length - 1; i++)
        {
            dsCart.ViewItemCartRow drItemCart = (dsCart.ViewItemCartRow)tblCart.Rows[i];
            RepeaterItem item = (RepeaterItem)repSparePartsConsumables.Items[i];
            HtmlTableRow tr = (HtmlTableRow)item.FindControl("trRow");
            tr.Attributes.Add("class", (i % 2 == 0) ? "gridData" : "gridDataAlt");

            Literal litNo = (Literal)item.FindControl("litNo");
            litNo.Text = (i + 1).ToString() + ".";

            Literal litEDPCode = (Literal)item.FindControl("litEDPCode");
            litEDPCode.Text = drItemCart.Code;

            Literal litName = (Literal)item.FindControl("litName");
            litName.Text = drItemCart.Name;

            Literal litUnitPrice = (Literal)item.FindControl("litUnitPrice");
            litUnitPrice.Text = drItemCart.UnitPrice.ToString(WebLib.CurrencyFormat);

            TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
            txtQuantity.Text = drItemCart.Quantity.ToString();

            Literal litTotalPrice = (Literal)item.FindControl("litTotalPrice");
            litTotalPrice.Text = (drItemCart.UnitPrice * drItemCart.Quantity).ToString(WebLib.CurrencyFormat);

            LinkButton lnkUpdateCart = (LinkButton)item.FindControl("lnkUpdateCart");
            lnkUpdateCart.CommandArgument = drItemCart.ID.ToString();

            LinkButton lnkRemove = (LinkButton)item.FindControl("lnkRemove");
            lnkRemove.CommandArgument = drItemCart.ID.ToString();

            totalQuantitySparePartsConsumables += drItemCart.Quantity;
            totalPriceSparePartsConsumables += drItemCart.Quantity * drItemCart.UnitPrice;
        }

        litSparePartsConsumablesTotalQuantity.Text = totalQuantitySparePartsConsumables.ToString();
        litSparePartsConsumablesTotalPrice.Text = "RM " + totalPriceSparePartsConsumables.ToString(WebLib.CurrencyFormat);
    }


    protected void repMachine_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "UPDATE")
        {
            UpdateCart(int.Parse(e.CommandArgument.ToString()), e.Item, CartType.Machine);
            DisplayCarts();
        }
        else if (e.CommandName == "REMOVE")
        {
            RemoveCart(int.Parse(e.CommandArgument.ToString()), e.Item);
            da.CartOptionalItem_DeleteByCartID(int.Parse(e.CommandArgument.ToString()));

            DisplayCarts();
        }
    }

    protected void repSparePartsConsumables_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "UPDATE")
        {
            UpdateCart(int.Parse(e.CommandArgument.ToString()), e.Item, CartType.SparePartConsumable);
            DisplayCarts();
        }
        else if (e.CommandName == "REMOVE")
        {
            RemoveCart(int.Parse(e.CommandArgument.ToString()), e.Item);
            DisplayCarts();
        }
    }

    void UpdateCart(int id, RepeaterItem item, CartType mode)
    {
        dsCart.CartRow drCart = da.Cart_GetDataByID(id);
        if (drCart != null)
        {
            #region Optional Items
            // Only applicable to machine
            if (mode == CartType.Machine)
            {
                Repeater repOptional = (Repeater)item.FindControl("repOptional");
                foreach (RepeaterItem itemOptional in repOptional.Items)
                {
                    HiddenField hidOptionalItemID = (HiddenField)itemOptional.FindControl("hidOptionalItemID");

                    TextBox txtOptionalQuantity = (TextBox)itemOptional.FindControl("txtOptionalQuantity");

                    dsCartOptionalItem.CartOptionalItemRow cartOptionalRow = da.CartOptionalItem_Get(int.Parse(hidOptionalItemID.Value));
                    if (Common.IsNum(txtOptionalQuantity.Text) && int.Parse(txtOptionalQuantity.Text) <= 9999)
                        cartOptionalRow.Quantity = int.Parse(txtOptionalQuantity.Text);
                    else
                        cartOptionalRow.Quantity = 1;

                    // Remove Item
                    if (cartOptionalRow.Quantity == 0)
                        cartOptionalRow.Delete();

                    da.CartOptionalItem_Update(cartOptionalRow);
                }
            }
            #endregion

            TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
            if (Common.IsNum(txtQuantity.Text) && int.Parse(txtQuantity.Text) <= 9999)
                drCart.Quantity = int.Parse(txtQuantity.Text);
            else
                drCart.Quantity = 1;
            
            // Remove Item
            if (drCart.Quantity == 0)
                drCart.Delete();

            da.Cart_Update(drCart);

            MasterBase.Message = WebLib.UpdatedSuccessfullyMessage;
        }
    }

    void RemoveCart(int id, RepeaterItem item)
    {
        dsCart.CartRow drCart = da.Cart_GetDataByID(id);
        if (drCart != null)
        {
            da.CartOptionalItem_DeleteByCartID(drCart.ID);

            drCart.Delete();
            da.Cart_Update(drCart);

            MasterBase.Message = WebLib.DeletedSuccessfullyMessage;
        }
    }

    protected void btnCheckOut_Click(object sender, EventArgs e)
    {
        List<ExpiredItemsClass> expiredItems = new List<ExpiredItemsClass>();
        dsCart.CartDataTable tblCart = da.Cart_GetByDealerUserID(WebLib.LoggedInUser.DealerUserID);
        foreach (dsCart.CartRow drCart in tblCart.Rows)
        {
            if (drCart.IsProduct)
            {
                dsProduct.ProductRow drProduct = da.Product_Get(drCart.CartItemID);
                if (drProduct.EffectiveDateFrom > DateTime.Today ||
                    drProduct.EffectiveDateTo < DateTime.Today)
                {
                    dsMachineItem.MachineItemDataTable tblMachineItem = da.MachineItem_Search(drProduct.ID, (short)ProductItemType.Machine, true);
                    if (tblMachineItem.Rows.Count > 0)
                    {
                        dsMachineItem.MachineItemRow drMachineItem = (dsMachineItem.MachineItemRow)tblMachineItem.Rows[0];
                        expiredItems.Add(new ExpiredItemsClass(drMachineItem.Code, drProduct.Name + " (" + drProduct.Code + ")"));
                    }
                }
            }
        }

        if (expiredItems.Count > 0)
        {
            repExpiredItems.DataSource = new byte[expiredItems.Count];
            repExpiredItems.DataBind();

            for (int i = 0; i < expiredItems.Count; i++)
            {
                RepeaterItem item = (RepeaterItem)repExpiredItems.Items[i];
                ExpiredItemsClass itemClass = expiredItems[i];

                Literal litEDPCode = (Literal)item.FindControl("litEDPCode");
                litEDPCode.Text = itemClass.EDPCode;

                Literal litName = (Literal)item.FindControl("litName");
                litName.Text = itemClass.Name;
            }

            OpenJS();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "ExpiredItems", "return confirm('" + sb.ToString() + ".<br/>Proceed to remove the items from the cart.');document.getElementById('" + btnRemoveExpiredItems.ClientID + "').click();", true);
        }
        else
            Response.Redirect("Checkout.aspx");
    }

    protected void btnRemoveExpiredItems_Click(object sender, EventArgs e)
    {
        List<int> deleteCartItems = new List<int>();

        dsCart.CartDataTable tblCart = da.Cart_GetByDealerUserID(WebLib.LoggedInUser.DealerUserID);
        foreach (dsCart.CartRow drCart in tblCart.Rows)
        {
            if (drCart.IsProduct)
            {
                dsProduct.ProductRow drProduct = da.Product_Get(drCart.CartItemID);
                if (drProduct.EffectiveDateFrom > DateTime.Today ||
                    drProduct.EffectiveDateTo < DateTime.Today)
                {
                    deleteCartItems.Add(drCart.ID);
                }
            }
            else
            {
                
            }
        }

        if (deleteCartItems.Count > 0)
        {
            foreach (int i in deleteCartItems)
            {
                dsCart.CartRow drCart = da.Cart_GetDataByID(i);
                if (drCart != null)
                {
                    da.CartOptionalItem_DeleteByCartID(drCart.ID);
                    drCart.Delete();

                    da.Cart_Update(drCart);
                }
            }            

            MasterBase.Message = "Expired items removed successfully.";
        }
        else
        {
            MasterBase.Message = "No expired items to remove.";
        }
        CloseJS();
        DisplayCarts();
    }
    protected void btnCancelExpiredItems_Click(object sender, EventArgs e)
    {
        CloseJS();
    }
    protected void btnRef_Click(object sender, EventArgs e)
    {

    }
    void CloseJS()
    {
        string js = "closeDialog(); ";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "select_" + ClientID, "<script>" + js + "</script>", false);
    }
    void OpenJS()
    {
        string js = "showDialog();";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "show_" + ClientID, "<script>" + js + "</script>", false);
    }
    
}
public class ExpiredItemsClass
{
    public string EDPCode = "";
    public string Name = "";
    public ExpiredItemsClass(string _edpcode, string _name)
    {
        EDPCode = _edpcode;
        Name = _name;
    }    
}
