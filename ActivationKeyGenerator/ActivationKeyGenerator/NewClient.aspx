<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewClient.aspx.cs" Inherits="ActivationKeyGenerator.NewClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>TRINITY Activation Key Generator - New Client</title>
    <style type="text/css">
        </style>
    <link href="StyleSheet1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" autocomplete="on" defaultbutton="SaveSingleBttn" defaultfocus="ClientDDL">
        <div>
            <p style="width: initial" class="auto-style1">
                <asp:Table ID="Table1" runat="server" CellPadding="10">
                    <asp:TableRow>
                        <asp:TableCell><a href="http://www.bradyplc.com" />
                            <asp:Image ID="BradyLogo" runat="server" ImageUrl="~/Images/smaller.JPG" Height="55" /></asp:TableCell>
                        <asp:TableCell Text="TRINITY Activation Key Generator" Font-Size="XX-Large" Font-Bold="True"></asp:TableCell>

                    </asp:TableRow>
                </asp:Table>

                <asp:Table ID="Table2" runat="server" CssClass="auto-style1" CellPadding="6" Width="287px">
                    <asp:TableRow>
                        <asp:TableCell Text="Create New Client" ColumnSpan="4" Font-Bold="True" Font-Size="X-Large"></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="New Client" HorizontalAlign="Right"></asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <asp:TextBox ID="NewClientTB" runat="server" AutoPostBack="true" Height="20px" TabIndex="1" Width="200"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="NewClientComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="Entities" VerticalAlign="Top" HorizontalAlign="Right"></asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <asp:RadioButtonList ID="EntityBL" runat="server" OnSelectedIndexChanged="EntityBL_SelectedIndexChanged" AutoPostBack="true" TabIndex="12" ToolTip="Single or multiple instances of this Client">
                                <asp:ListItem>Single</asp:ListItem>
                                <asp:ListItem>Multiple</asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="EntitiesComment" runat="server" ForeColor="Red" Visible="false"></asp:Label></asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow ID="NewEntityID" Visible="false">
                        <asp:TableCell Text="New Entity Name" HorizontalAlign="Right"></asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <asp:TextBox ID="NewEntityNameTB" runat="server" AutoPostBack="true" Width="200px" TabIndex="2" ToolTip="Name of current entity"></asp:TextBox></asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="NewEntityNameComment" runat="server" ForeColor="Red" Visible="false"></asp:Label></asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow>
                        <asp:TableCell Text="Version" HorizontalAlign="Right"></asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <asp:TextBox ID="NewVersionTB" runat="server" Width="40px" TabIndex="2" ToolTip="Software version of Trinity. ie 2013"></asp:TextBox></asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="NewVersionComment" runat="server" ForeColor="Red" Visible="false"></asp:Label></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="Last SP Delivered" HorizontalAlign="Right"></asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <asp:TextBox ID="NewLastSPTB" runat="server" Width="40px" TabIndex="3" ToolTip="The last Service Pack delivered. Changed manually."></asp:TextBox></asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="NewLastSPComment" runat="server" ForeColor="Red" Visible="false"></asp:Label></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="Email to:" HorizontalAlign="Right"></asp:TableCell>
                        <asp:TableCell ColumnSpan="3">
                            <asp:TextBox ID="NewEmailToTB" runat="server" Width="615" TabIndex="4"></asp:TextBox></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="System Code" Font-Size="Medium" Font-Names="Sans-Serif" HorizontalAlign="Right"></asp:TableCell>
                        <asp:TableCell ColumnSpan="1">
                            <asp:Label ID="NewSystemCode" runat="server" ></asp:Label></asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <asp:Label ID="NewSystemCodeComment" runat="server" ForeColor="Red" Visible="false"></asp:Label></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="Activation Type" VerticalAlign="Top" HorizontalAlign="Right"></asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <asp:RadioButtonList ID="NewActivationTypeBL" runat="server" TabIndex="6">
                                <asp:ListItem Selected="True">Concurrent users</asp:ListItem>
                                <asp:ListItem>Named users</asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="NewActivationTypeComment" runat="server" ForeColor="Red" Visible="false"></asp:Label></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="Interactive Licenses" HorizontalAlign="Right"></asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <asp:TextBox ID="NewInteractiveLicensesTB" runat="server" Width="40px" TabIndex="7"></asp:TextBox></asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="NewInteractiveLicensesComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="Service Licenses" HorizontalAlign="Right"></asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <asp:TextBox ID="NewServiceLicensesTB" runat="server" Width="40px" TabIndex="8"></asp:TextBox></asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="NewServiceLicensesComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="Expiry" HorizontalAlign="Right"></asp:TableCell>
                        <asp:TableCell Text="Month   "  Width="10px" HorizontalAlign="right">
                            <asp:TextBox ID="NewExpiryMonthTB" runat="server" Width="40px" TabIndex="9" ToolTip ="Month of expiry.  Must be of the form MM."></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="NewExpiryMonthComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell></asp:TableCell>
                        <asp:TableCell Text="Year   " Width="10px" HorizontalAlign="right">
                             <asp:TextBox ID="NewExpiryYearTB" runat="server" Width="40px" TabIndex="10" ToolTip="Year of expiry.  Must be of the form YYYY."></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="NewExpiryYearComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="MEBLRow" Visible="false">
                        <asp:TableCell>
                            <asp:Button ID="SaveMultipleBttn" runat="server" Enabled="false" Style="cursor: pointer" Text="Save New Client" Width="141px" OnClick="SaveMultipleBttn_Click" TabIndex="11" ToolTip="Select after entering all entities." /></asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <asp:Button ID="AddNewBttn" runat="server" Enabled="false" Style="cursor: pointer" Text="Add New/Save" Width="141px" OnClick="AddNewBttn_Click" TabIndex="11" ToolTip="Save this entity, add new entity"/></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="SaveSingleBttn" runat="server" Enabled="false" Style="cursor: pointer" Text="Save New Client" Width="141px" OnClick="SaveSingleBttn_Click" TabIndex="11" ToolTip="Select after entering all fields" /></asp:TableCell>
                        <asp:TableCell ColumnSpan="1">
                            <asp:Button ID="GoBack" runat="server" Style="cursor: pointer" Text="Go Back" OnClick="GoBack_Click" Width="141px" TabIndex="12" ToolTip="Return to the Activation Key Generator." /></asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="ResetBttn" runat="server" Enabled="false" Style="cursor: pointer" Text="Reset" OnClick="ResetBttn_Click" Width="141px" TabIndex="12" ToolTip="Clear all but New Client name" /></asp:TableCell>
                    </asp:TableRow>

                </asp:Table>
            </p>
        </div>
    </form>
</body>
</html>

