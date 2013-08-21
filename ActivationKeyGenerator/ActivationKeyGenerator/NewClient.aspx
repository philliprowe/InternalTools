<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewClient.aspx.cs" Inherits="ActivationKeyGenerator.NewClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Trinity Activation Key Generator - New Client</title>
    <style type="text/css">
        </style>
    <link href="StyleSheet1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" autocomplete="on" DefaultButton="SaveNewClientBttn" defaultfocus="ClientDDL">
    <div>
       <p style="width:initial" class="auto-style1">
            <asp:Table ID="Table1" runat="server" CellPadding="10">
                <asp:TableRow>
                    <asp:TableCell><asp:Image ID="BradyLogo" runat="server" ImageUrl="~/Images/smaller.JPG" Height="55" /></asp:TableCell>
                    <asp:TableCell Text="Trinity Activation Key Generator" Font-Size="XX-Large" Font-Bold="True" ></asp:TableCell>
                   
                </asp:TableRow>
            </asp:Table>
              
            <asp:Table ID="Table2" runat="server" CssClass="auto-style1" CellPadding="5" >
                <asp:TableRow>
                    <asp:TableCell Text="Create New Client" ColumnSpan="4" Font-Bold="True" Font-Size="X-Large"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow >
                    <asp:TableCell Text="New Client"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:TextBox ID="NewClientTB" runat="server" Height="20px" TabIndex="1" Width="200"></asp:TextBox> </asp:TableCell>
                    <asp:TableCell ><asp:Label ID="NewClientLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Version"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:TextBox ID="NewVersionTB" runat="server" Width="40px" TabIndex="2" ToolTip="Software version of Trinity. ie 2013"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Last SP Delivered"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:TextBox ID="NewLastSPTB" runat="server" Width="40px" TabIndex="3" ToolTip="The last Service Pack delivered. Changed manually."></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Email to:"></asp:TableCell>
                    <asp:TableCell ColumnSpan="3"> <asp:TextBox ID="NewEmailToTB" runat="server" Width="615" TabIndex="4" ></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text ="System Code" Font-Size="Medium" Font-Names="Sans-Serif"  HorizontalAlign="Left"></asp:TableCell>
                    <asp:TableCell ColumnSpan="3"><asp:TextBox ID="NewSystemCodeTB" runat="server" Width="615"  TabIndex="5" ToolTip="Must be exact customer name as per SFTP. Multiple system codes per client can be separated by a comma."></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Activation Type" VerticalAlign="Top"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
                        <asp:RadioButtonList ID="NewActivationTypeBL" runat="server" TabIndex="6">
                        <asp:ListItem Selected ="True">Concurrent users</asp:ListItem>
                        <asp:ListItem>Named users</asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:TableCell>
                    <asp:TableCell><asp:Label ID="NewActivationTypeLabel" runat ="server" ForeColor="Red" Visible="false"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Limit"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:TextBox ID="NewLimitTB" runat="server" Width="40px" TabIndex="7"></asp:TextBox></asp:TableCell>
                    <asp:TableCell><asp:Label ID="NewLimitLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Extended Limit"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:TextBox ID="NewExtendedLimitTB" runat="server" Width="40px" TabIndex="8"></asp:TextBox></asp:TableCell>
                    <asp:TableCell><asp:Label ID="NewExtendedLimitLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                    </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text ="Expiry"></asp:TableCell>
                    <asp:TableCell Text ="Month" Width="30" HorizontalAlign="Right"></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left"><asp:TextBox ID="NewExpiryMonthTB" runat="server" Width="40px" TabIndex="9" ToolTip="Month of expiry.  Must be of the form MM."></asp:TextBox></asp:TableCell>
                    <asp:TableCell><asp:Label ID="NewExpiryMonthLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell Text ="Year" HorizontalAlign="Right"></asp:TableCell>
                    <asp:TableCell HorizontalAlign ="Left"><asp:TextBox ID="NewExpiryYearTB" runat="server" Width="40px" TabIndex="10" ToolTip="Year of expiry.  Must be of the form YYYY."></asp:TextBox></asp:TableCell>
                    <asp:TableCell><asp:Label ID="NewExpiryYearLabel" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell><asp:Button ID="SaveNewClientBttn" runat="server" Text="Save New Client" Width="141px" OnClick="SaveNewClientBttn_Click" TabIndex="11" /></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:Button ID="GoBack" runat="server" Text="Go Back" OnClick="GoBack_Click" Width="141px" TabIndex="12" ToolTip="Return to the Activation Key Generator." /></asp:TableCell>
                </asp:TableRow>
               
        </asp:Table>
           </p>
            </div>
    </form>
</body>
</html>

