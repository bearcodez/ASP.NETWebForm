<%@ Page Title="My Cart" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="MyCart.aspx.cs" Inherits="MyCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h2>
        My Cart</h2>
    <asp:Panel ID="pnlNoRecord" runat="server" Visible="false">
        <div class="Message" style="width: 60%">
            Your cart is empty. Please add some items to your cart.
        </div>
    </asp:Panel>
    <style>
        .Grid
        {
            border: 1px solid #fff;
        }
        .GridRow2 td
        {
            background-color: #fff;
            border-right:1px solid #fff;
            border-left:1px solid #fff;
            padding: 5px;
        }
        .GridRowAlt2 td
        {
            background-color: #eee;
            border-right:1px solid #fff;
            border-left:1px solid #fff;
            padding: 5px;
        }
    </style>
    <asp:Panel ID="pnlListing" runat="server">
        <asp:Panel ID="pnlMachine" runat="server">
            <h3>
                Machines/Accessories</h3>
            <table width="100%">
                <tr class="GridHeader">
                    <td width="30px">
                        No.
                    </td>
                    <td width="80px">
                        EDP Code
                    </td>
                    <td width="290px">
                        Product Name
                    </td>
                    <td width="130px">
                        Unit Price
                    </td>
                    <td width="105px">
                        Quantity
                    </td>
                    <td width="95px">
                        Total Price
                    </td>
                    <td width="110px">
                    </td>
                </tr>
                <asp:Repeater ID="repMachine" runat="server" OnItemCommand="repMachine_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td colspan="7">
                                <table cellspacing="0" cellpadding="0" width="100%">
                                    <tr id="trRow" runat="server">
                                        <td valign="top" align="center" width="30px">
                                            <asp:HiddenField ID="hidIsNoTier" runat="server" />
                                            <asp:Literal ID="litNo" runat="server"></asp:Literal>
                                        </td>
                                        <td valign="top" width="80px">
                                            <asp:Literal ID="litEDPCode" runat="server"></asp:Literal>
                                        </td>
                                        <td valign="top" width="290px">
                                            <asp:Literal ID="litName" runat="server"></asp:Literal>
                                        </td>
                                        <td valign="top" align="right" width="130px">
                                            <asp:Literal ID="litUnitPrice" runat="server"></asp:Literal>
                                        </td>
                                        <td valign="top" align="right" width="105px">
                                            <asp:TextBox ID="txtQuantity" runat="server" Width="50" Style="text-align: right" MaxLength="4"></asp:TextBox>
                                        </td>
                                        <td valign="top" align="right" width="95px">
                                            <asp:Literal ID="litTotalPrice" runat="server"></asp:Literal>
                                        </td>
                                        <td valign="top" align="center" width="110px">
                                            <asp:LinkButton ID="lnkUpdateCart" runat="server" Text="Update" CommandName="UPDATE" />
                                            |
                                            <asp:LinkButton ID="lnkRemove" runat="server" CommandName="REMOVE" Text="Remove" OnClientClick="return confirm('Are you sure want to remove this item?');"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <asp:Repeater ID="repMachineItem" runat="server">
                                        <HeaderTemplate>
                                            <tr id="trRow" runat="server">
                                                <td></td>
                                                <td></td>
                                                <td><b><u>Machine</u></b></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr id="trRow" runat="server">
                                                <td valign="top" align="right">
                                
                                                </td>
                                                <td valign="top">
                                                    <asp:Literal ID="litMachineEDPCode" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="top">
                                                    <asp:Literal ID="litMachineName" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="top" align="right">
                                        
                                                </td>
                                                <td valign="top" align="right">
                                        
                                                </td>
                                                <td valign="top" align="right">
                                
                                                </td>
                                                <td valign="top" align="center">

                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:Repeater ID="repService" runat="server">
                                        <HeaderTemplate>
                                            <tr id="trRow" runat="server">
                                                <td></td>
                                                <td></td>
                                                <td><b><u>Service</u></b></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr id="trRow" runat="server">
                                                <td valign="top" align="right">
                                
                                                </td>
                                                <td valign="top">
                                                    <asp:Literal ID="litServiceEDPCode" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="top">
                                                    <asp:Literal ID="litServiceName" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="top" align="right">
                                        
                                                </td>
                                                <td valign="top" align="right">
                                        
                                                </td>
                                                <td valign="top" align="right">
                                
                                                </td>
                                                <td valign="top" align="center">

                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:Repeater ID="repConfig" runat="server">
                                        <HeaderTemplate>
                                            <tr id="trRow" runat="server">
                                                <td></td>
                                                <td></td>
                                                <td><b><u>Config</u></b></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr id="trRow" runat="server">
                                                <td valign="top" align="right">
                                
                                                </td>
                                                <td valign="top">
                                                    <asp:Literal ID="litConfigEDPCode" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="top">
                                                    <asp:Literal ID="litConfigName" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="top" align="right">
                                        
                                                </td>
                                                <td valign="top" align="right">
                                        
                                                </td>
                                                <td valign="top" align="right">
                                
                                                </td>
                                                <td valign="top" align="center">

                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                        
                                    <asp:Repeater ID="repOptional" runat="server">
                                        <HeaderTemplate>
                                            <tr id="trRow" runat="server">
                                                <td></td>
                                                <td></td>
                                                <td><b><u>Optional</u></b></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr id="trRow" runat="server">
                                                <td valign="top" align="right">
                                                    <asp:HiddenField ID="hidOptionalItemID" runat="server" />
                                                </td>
                                                <td valign="top">
                                                    <asp:Literal ID="litOptionalEDPCode" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="top">
                                                    <asp:Literal ID="litOptionalName" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="top" align="right">
                                                    <asp:Literal ID="litOptionalUnitPrice" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="top" align="right">
                                                    <asp:TextBox ID="txtOptionalQuantity" runat="server" Width="50" Style="text-align: right" MaxLength="4"></asp:TextBox>
                                                </td>
                                                <td valign="top" align="right">
                                
                                                </td>
                                                <td valign="top" align="center">

                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:Repeater ID="repOthers" runat="server">
                                        <HeaderTemplate>
                                            <tr id="trRow" runat="server">
                                                <td></td>
                                                <td></td>
                                                <td><b><u>Others</u></b></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr id="trRow" runat="server">
                                                <td valign="top" align="right">
                                
                                                </td>
                                                <td valign="top">
                                                    <asp:Literal ID="litOthersEDPCode" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="top">
                                                    <asp:Literal ID="litOthersName" runat="server"></asp:Literal>
                                                </td>
                                                <td valign="top" align="right">
                                        
                                                </td>
                                                <td valign="top" align="right">
                                        
                                                </td>
                                                <td valign="top" align="right">
                                
                                                </td>
                                                <td valign="top" align="center">

                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </td>
                        </tr>
                        
                    </ItemTemplate>
                </asp:Repeater>
                
                <tr>
                    <td colspan="7" style="text-align: right">
                        <table align="right" width="500px" style="border-top: 1px solid #bbb; font-weight: bold; font-size: 16px;">
                            <tr>
                                <td>
                                    Total Machines/Accessories Quantity :
                                </td>
                                <td width="150px" style="text-align: right">
                                    <asp:Literal ID="litMachineTotalQuantity" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Total Price :
                                </td>
                                <td style="text-align: right">
                                    <asp:Literal ID="litMachineTotalPrice" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </asp:Panel>
        <asp:Panel ID="pnlSparePartsConsumables" runat="server">
            <h3>
                Spare Parts & Consumables</h3>
            <table width="100%">
                <tr class="GridHeader">
                    <td>
                        No.
                    </td>
                    <td width="100px">
                        EDP Code
                    </td>
                    <td width="250px">
                        Product Name
                    </td>
                    <td>
                        Unit Price
                    </td>
                    <td>
                        Quantity
                    </td>
                    <td>
                        Total Price
                    </td>
                    <td>
                    </td>
                </tr>
                <asp:Repeater ID="repSparePartsConsumables" runat="server" OnItemCommand="repSparePartsConsumables_ItemCommand">
                    <ItemTemplate>
                        <tr id="trRow" runat="server">
                            <td valign="top" align="center">
                                <asp:Literal ID="litNo" runat="server"></asp:Literal>
                            </td>
                            <td valign="top">
                                <asp:Literal ID="litEDPCode" runat="server"></asp:Literal>
                            </td>
                            <td valign="top">
                                <asp:Literal ID="litName" runat="server"></asp:Literal>
                            </td>
                            <td valign="top" align="right">
                                <asp:Literal ID="litUnitPrice" runat="server"></asp:Literal>
                            </td>
                            <td valign="top" align="right">
                                <asp:TextBox ID="txtQuantity" runat="server" Width="50" Style="text-align: right" MaxLength="4"></asp:TextBox>
                            </td>
                            <td valign="top" align="right">
                                <asp:Literal ID="litTotalPrice" runat="server"></asp:Literal>
                            </td>
                            <td valign="top" align="center">
                                <asp:LinkButton ID="lnkUpdateCart" runat="server" Text="Update" CommandName="UPDATE" />
                                |
                                <asp:LinkButton ID="lnkRemove" runat="server" CommandName="REMOVE" Text="Remove" OnClientClick="return confirm('Are you sure want to remove this item?');"></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td colspan="7" style="text-align: right">
                        <table align="right" width="600px" style="border-top: 1px solid #bbb; font-weight: bold; font-size: 16px;">
                            <tr>
                                <td>
                                    Total Spare Parts and Consumables Quantity :
                                </td>
                                <td width="150px" style="text-align: right">
                                    <asp:Literal ID="litSparePartsConsumablesTotalQuantity" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Total Price :
                                </td>
                                <td style="text-align: right">
                                    <asp:Literal ID="litSparePartsConsumablesTotalPrice" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </asp:Panel>
        <table width="100%">
            <tr>
                <td style="text-align: right">
                    <table align="right" width="400px" style="border-top: 1px solid #bbb; font-weight: bold; font-size: 16px;">
                        <tr>
                            <td>
                                Grand Total Quantity :
                            </td>
                            <td width="150px" style="text-align: right">
                                <asp:Literal ID="litGrandTotalQuantity" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Grand Total Price :
                            </td>
                            <td style="text-align: right">
                                <asp:Literal ID="litGrandTotalPrice" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: right">                    
                    <asp:Button ID="btnCheckOut" runat="server" Text="Check Out" CssClass="CheckOut" OnClick="btnCheckOut_Click" />
                    <br />
                    <br />
                    <a href="ProductMachines.aspx">Continue Shopping</a>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div id="divExpiredItems" style="VISIBILITY:hidden; DISPLAY:none; width:100%;   ">
            <table > 
                <tr>
                    <td class="formSectionTitle">The following items had expired:-</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnRef" OnClick="btnRef_Click" runat="server" style="display:none" Text="Button" />
                        <table>
                            <tr class="GridHeader">
                                <td style="width:100px">EDP Code</td>
                                <td style="width:300px">Name</td>
                            </tr>
                            <asp:Repeater ID="repExpiredItems" runat="server">
                                <ItemTemplate>
                                    <tr id="trRow" runat="server">
                                        <td><asp:Literal ID="litEDPCode" runat="server"></asp:Literal></td>
                                        <td><asp:Literal ID="litName" runat="server"></asp:Literal></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="right" class="formSectionButtons">
                        <asp:Button ID="btnRemoveExpiredItems" OnClick="btnRemoveExpiredItems_Click" runat="server" Text="Remove" CssClass="B8" />
                        <asp:Button ID="btnCancelExpiredItems" OnClick="btnCancelExpiredItems_Click" runat="server" Text="Cancel" CssClass="B8" />
                    </td>
                </tr>
            </table>
        </div>
    <script type="text/javascript">
        $(document).ready(function () {
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
            $("input[id*=txtOptionalQuantity]").each(function () {
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

        function showDialog() {
            var divId = "divExpiredItems";
            document.getElementById(divId).style.visibility = "visible";
            document.getElementById(divId).style.display = "block";
            $(function () {
                $("#" + divId).dialog({
                    modal: true,
                    width: 450,
                    open: function (type, data) {
                        $(this).parent().appendTo("form");
                    }
                });
            });
        }
        function closeDialog() {
            var divId = "divExpiredItems";
        
            document.getElementById(divId).style.visibility = "hidden";
            document.getElementById(divId).style.display = "none";
        }
    </script> 
</asp:Content>
