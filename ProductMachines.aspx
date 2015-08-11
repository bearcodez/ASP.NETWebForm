<%@ Page Title="Browse Machines" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProductMachines.aspx.cs" Inherits="ProductMachines" %>

<%@ Register Src="UserControls/ListingPageControl.ascx" TagName="ListingPageControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:HiddenField ID="hidCartID" runat="server" />
    <asp:Panel ID="pnlMachineListing" runat="server">
        <h2>
            Machines</h2>
        <br />
        <asp:Panel ID="pnlSearch" runat="server">
            <table align="center" cellpadding="2">
                <tr>
                    <td>
                        Name :
                    </td>
                    <td>
                        <asp:TextBox ID="txtSearchName" runat="server" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Product Code :
                    </td>
                    <td>
                        <asp:TextBox ID="txtSearchCode" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Recon :
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rdRecon" runat="server" RepeatDirection="Horizontal">
                        </asp:RadioButtonList>
                        <asp:HiddenField ID="hidRdRecon" runat="server" />
                    </td>
                </tr>
                 <tr>
                    <td>
                        Is Promotion :
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rdIsPromotion" runat="server" RepeatDirection="Horizontal">
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="B8" OnClick="btnSearch_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlListing" runat="server" Visible="false">
            <table cellpadding="10" style="width: 100%">
                <tr class="GridHeader">
                    <td>
                        <b>Product Code</b>
                    </td>
                    <td style="width: 300px">
                        <b>Product Name</b>
                    </td>                    
                    <td style="width: 150px">
                        <b>Order Type</b>
                    </td>
                    <td style="width: 130px">
                        <b>Unit Price (RM)</b>
                    </td>
                    <td>
                        <b>Quantity</b>
                    </td>
                    <td>
                    </td>
                </tr>
                <asp:Repeater ID="rep" runat="server" OnItemCommand="rep_ItemCommand">
                    <ItemTemplate>
                        <tr id="trRow" runat="server">
                            <td valign="top">
                                <asp:HiddenField ID="hidProductID" runat="server" /><asp:CheckBox Visible="false" ID="chk" runat="server" AutoPostBack="false" />
                                <asp:Literal ID="litCode" runat="server"></asp:Literal>
                            </td>
                            <td valign="top">
                                <b>
                                    <asp:LinkButton ID="lnkDetails" runat="server" CommandName="PRODUCTDETAIL"></asp:LinkButton></b>
                            </td>
                            
                            <td valign="top">
                                <asp:RadioButtonList RepeatLayout="Flow" ID="rdOrderType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Normal" Value="Normal" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                                </asp:RadioButtonList>
                                [<a id="OtherToolTip<%#Container.ItemIndex %>" href="#" tooltip="eg. (Bulk/Promotional/Tender). Enter your own price for this type of purchase.">?</a>]
                            </td>
                            <td valign="top">
                                <asp:Label ID="lblPrice" runat="server"></asp:Label>
                                <asp:TextBox ID="txtPrice" runat="server" Width="60px" Style="display: none; text-align: right"></asp:TextBox>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtQuantity" runat="server" Width="50" Text="1" Style="text-align: right" MaxLength="4"></asp:TextBox>
                            </td>
                            <td valign="top">
                                <asp:Button CssClass="Cart" ID="btnAddToCart" runat="server" Text="Add to Cart" OnClick="btnAddToCart_Click" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>                
            </table>
            <uc1:ListingPageControl ID="pgc" runat="server" />
        </asp:Panel>        
    </asp:Panel>
    <asp:Panel ID="pnlDetails" runat="server" Visible="false">
        <h2>
            Product Details</h2>
        <table style="width: 900px">
            <tr>
                <td>
                    <h1>
                        <asp:Literal ID="litProductName" runat="server"></asp:Literal></h1>
                    <asp:Literal ID="litProductCode" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td><h3>Machine</h3>
                    <asp:Repeater ID="repMachine" runat="server">
                        <ItemTemplate>
                            <asp:Literal ID="litMachineName" runat="server"></asp:Literal><br />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <asp:Panel ID="pnlStandardConfig" runat="server">
            <tr>
                <td>
                    <h3>
                        Standard Configuration</h3>
                    <asp:Repeater ID="repConfig" runat="server">
                        <ItemTemplate>
                            <asp:Literal ID="litProductItemTypeConfig" runat="server"></asp:Literal><br />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="pnlStandardService" runat="server">
            <tr>
                <td>
                    <h3>
                        Standard Service</h3>
                    <asp:Repeater ID="repService" runat="server">
                        <ItemTemplate>
                            <asp:Literal ID="litProductItemTypeService" runat="server"></asp:Literal><br />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="pnlOptional" runat="server">
            <tr>
                <td>
                    <h3>
                        Optional</h3>
                    <asp:CheckBoxList ID="chkProductItemTypeOptional" Visible="false" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" CellPadding="5" BorderWidth="1">
                    </asp:CheckBoxList>
                    <asp:Panel ID="pnlDetailsOptional" runat="server">
                     <table width="600px">
                        <tr class="GridHeader">
                            <td></td>
                            <td width="100px">
                                EDP Code
                            </td>
                            <td width="250px">
                                Item Name
                            </td>
                            <td>
                                Unit Price (RM)
                            </td>
                            <td>
                                Quantity
                            </td>
                        </tr>
                        <asp:Repeater ID="repDetailsOptional" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><asp:HiddenField ID="hidDetailsOptionalItemID" runat="server" /><asp:CheckBox ID="chk" runat="server" AutoPostBack="false" /></td>
                                    <td valign="top"><asp:Literal ID="litDetailsEDPCode" runat="server"></asp:Literal></td>
                                    <td valign="top"><asp:Literal ID="litDetailsName" runat="server"></asp:Literal></td>
                                    <td valign="top" align="right"><asp:Literal ID="litDetailsUnitPrice" runat="server"></asp:Literal></td>
                                    <td valign="top" align="right"><asp:TextBox ID="txtDetailsQuantity" runat="server" Width="50" Text="" Style="text-align: right" MaxLength="4"></asp:TextBox></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    </asp:Panel>
                </td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="pnlOthers" runat="server">
            <tr>
                <td>
                    <h3>
                        Others</h3>
                    <asp:Repeater ID="repOthers" runat="server">
                        <ItemTemplate>
                            <asp:Literal ID="litProductItemTypeOthers" runat="server"></asp:Literal><br />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            </asp:Panel>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-bottom: 3px">
                    <asp:Button ID="btnAddToCart" runat="server" CssClass="B12" Text="Add To Cart" OnClick="btnAddToCartDetails_Click"></asp:Button>
                    <asp:Button ID="btnBack" runat="server" CssClass="B8" Text="Back" OnClick="btnBack_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <script type="text/javascript">
        // Create the tooltips only on document load
        $(document).ready(function () {
            $("a[id*=OtherToolTip]").each(function () {
                $(this).qtip(
                    {
                        content: 'eg. <b>(Bulk/Promotional/Tender)</b>. Enter your own price for this type of purchase.', // Give it some content, in this case a simple string
                        style: 'cream'
                    });
            });

            $("input[id*=txtQuantity]").each(function () {
                $(this).keydown(function (event) {
                    // Allow only backspace and delete
                    if (event.keyCode == 46 || event.keyCode == 8) {
                        // let it happen, don't do anything
                    }
                    else {
                        // Ensure that it is a number and stop the keypress
                        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                            event.preventDefault();
                        }
                    }
                });
            });
        });

        function CheckedOrderType(id, lblPrice, txtPrice) {
            var table = document.getElementById(id);
            var radio = table.getElementsByTagName("input");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked && radio[i].value == "Others") {
                    document.getElementById(lblPrice).style.display = "none";
                    document.getElementById(txtPrice).style.display = "";
                }
                else {
                    document.getElementById(lblPrice).style.display = "";
                    document.getElementById(txtPrice).style.display = "none";
                }
            }
        }

        function CheckOptionalItemQuantity(id, quantityID) {
            var chk = document.getElementById(id);
            var txt = document.getElementById(quantityID);

            if (chk.checked) {
                if (txt.value == "") {
                    txt.value = "1";
                }
            }
            else {
                txt.value = "";
            }
        }
        </script>
</asp:Content>
