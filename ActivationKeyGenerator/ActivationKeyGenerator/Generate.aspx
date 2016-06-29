<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Generate.aspx.cs" Inherits="ActivationKeyGenerator.Generate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en-GB">
<head runat="server">
        <meta name="description" content="Page for generating a key to renew trinity software solution license from Brady Plc." /> 
        <meta name="author" content="Bradyplc." />
    <title>Trinity Activation Key Generator</title>
    <style type="text/css">
        </style>
    <link href="StyleSheet1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" autocomplete="on" DefaultButton="ActivationKeyBttn" defaultfocus="ClientDDL">
    <div>
       <p style="width:initial" class="auto-style1">
            <asp:Table ID="Table1" runat="server" CellPadding="10">
                <asp:TableRow>
                    <asp:TableCell><a href="http://www.bradyplc.com" /><asp:Image ID="BradyLogo" runat="server" ImageUrl="~/Images/smaller.JPG" Height="55" /></asp:TableCell>
                    <asp:TableCell Text="TRINITY Activation Key Generator" Font-Size="XX-Large" Font-Bold="True" ></asp:TableCell>
                   
                </asp:TableRow>
            </asp:Table>
            
            <asp:Table ID="Table2" runat="server" CssClass="auto-style1" CellPadding="5" >
                <asp:TableRow >
                    <asp:TableCell ID="ClientID" Text="Client" Visible="true"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:DropDownList ID="ClientDDL" runat="server" Style="cursor:pointer" Height="20px"  AutoPostBack="True" OnSelectedIndexChanged="ClientDDL_SelectedIndexChanged" TabIndex="1" Width="200" ></asp:DropDownList> </asp:TableCell>
                    <asp:TableCell ><asp:Label ID="ClientComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="EntityRow" Visible="false">
                    <asp:TableCell> <asp:Label ID="EntityComment1" runat="server" Text="Entity"></asp:Label></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:DropDownList ID="EntityDDL" runat="server" Style="cursor:pointer" Height="20px"  AutoPostBack="True" OnSelectedIndexChanged="EntityDDL_SelectedIndexChanged"  TabIndex="1" Width="200" ></asp:DropDownList> </asp:TableCell>
                    <asp:TableCell ><asp:Label ID="EntityComment2" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Version"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:TextBox ID="VersionTB" runat="server" Width="40px" TabIndex="2" ToolTip="Software version of Trinity. ie 2013"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Last SP Delivered"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:TextBox ID="LastSPTB" runat="server" Width="40px" TabIndex="3" ToolTip="The last Service Pack delivered.  Changed manually."></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Email to:"></asp:TableCell>
                    <asp:TableCell ColumnSpan="3"> <asp:TextBox ID="EmailToTB" runat="server" Width="615" TabIndex="4" ></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text ="Product Activation" ColumnSpan="4" Font-Bold="True" Font-Size="X-Large" Height="50" VerticalAlign="Bottom"></asp:TableCell> 
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text ="System Code" Font-Size="Medium" Font-Names="Sans-Serif"  HorizontalAlign="Left"></asp:TableCell>
                    <asp:TableCell ColumnSpan="3"><asp:TextBox ID="SystemCodeTB" runat="server" Width="615" OnTextChanged="SystemCodeTB_TextChanged" TabIndex="5" ToolTip="Must be exact customer name as SFTP.  Multiple system codes per client can be separated by a comma."></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Activation Type" VerticalAlign="Top"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
                        <asp:RadioButtonList ID="ActivationTypeBL" runat="server" TabIndex="6">
                        <asp:ListItem>Concurrent users</asp:ListItem>
                        <asp:ListItem>Named users</asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:TableCell>
                    <asp:TableCell><asp:Label ID="ActivationTypeComment" runat ="server" ForeColor="Red" Visible="false"></asp:Label> </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Interactive Licenses"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:TextBox ID="InteractiveTB" runat="server" Width="40px" TabIndex="7"></asp:TextBox></asp:TableCell>
                    <asp:TableCell><asp:Label ID="InteractiveComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Service Licenses"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:TextBox ID="ServiceTB" runat="server" Width="40px" TabIndex="8"></asp:TextBox></asp:TableCell>
                    <asp:TableCell><asp:Label ID="ServiceComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                    </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text ="Expiry"></asp:TableCell>
                    <asp:TableCell Text ="Month" Width="30" HorizontalAlign="Right"></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left"><asp:TextBox ID="ExpiryMonthTB" runat="server" Width="40px" TabIndex="9" ToolTip="Month of expiry.  Must be of the form MM."></asp:TextBox></asp:TableCell>
                    <asp:TableCell><asp:Label ID="ExpiryMonthComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell Text ="Year" HorizontalAlign="Right"></asp:TableCell>
                    <asp:TableCell HorizontalAlign ="Left"><asp:TextBox ID="ExpiryYearTB" runat="server" Width="40px" TabIndex="10" ToolTip="Year of expiry.  Must be of the form YYYY"></asp:TextBox></asp:TableCell>
                    <asp:TableCell><asp:Label ID="ExpiryYearComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell><asp:Button ID="AmendBttn" runat="server" Style="cursor:pointer" Text="Amend Client Details" Width="141px" OnClick="AmendBttn_Click" TabIndex="11" ToolTip="Save this Client's details." /></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:Button ID="NewClientBttn" runat="server" Style="cursor:pointer" Text="Create a new Client" OnClick="NewClientBttn_Click" Width="141px" TabIndex="20" ToolTip="Add a new Client." /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text ="Trinity Version"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:TextBox ID="ActivTrinityVersionTB" runat="server" Width="40px" TabIndex="12" ToolTip="The Trinity Version you want to create an Activation Key for."></asp:TextBox></asp:TableCell>
                    <asp:TableCell> <asp:Label ID="VersionComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Text="Service Pack"></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:TextBox ID="ActivServicePackTB" runat="server" Style="cursor:pointer" Width="40px" TabIndex="13" ToolTip="The Service Pack you wish to create an Activation Key for. Do not include patch number - each Activation Key is valid for a Service Pack."></asp:TextBox></asp:TableCell>
                    <asp:TableCell><asp:Label ID="ServicePackComment" runat="server" ForeColor="Red" Visible="False"></asp:Label></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3"><asp:Label ID="MulipleSystemsComment" runat="server" ForeColor="Red" Text="Please select the trading system you wish to activate:" Visible="False" TabIndex="14" ToolTip=""></asp:Label></asp:TableCell>
                    <asp:TableCell><asp:DropDownList ID="MultipleSystemsDDL" runat="server" Style="cursor:pointer" ForeColor="Red" Visible="False"></asp:DropDownList></asp:TableCell> 
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell><asp:Button ID="ActivationKeyBttn" runat="server" OnClick="ActivationKeyBttn_Click" Text="Show Activation Key" TabIndex="15" Width="141px" /></asp:TableCell>
                    <asp:TableCell ColumnSpan="3"><asp:TextBox ID="ActivationKeyTB" runat="server" Visible="False" Width="615px" ReadOnly="True" TabIndex="16"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell ColumnSpan="2"><asp:Button ID="PushsftpBttn" runat="server" Style="cursor:pointer"  OnClick="PushsftpBttn_Click" Text="Push Activation Key to SFTP" Visible="False" Width="200px" TabIndex="16" /></asp:TableCell>
                </asp:TableRow>
               </asp:Table>
        <asp:Table ID="Table3" runat="server" CssClass="auto-style1" CellPadding="5" >
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="ActivationComment" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                </asp:TableCell>
                </asp:TableRow>
        </asp:Table>
           <asp:Table ID ="Table4" runat ="server" CssClass ="auto-style1" CellPadding ="5">
               <asp:TableRow ID ="sftpRow" Visible="false">
                   <asp:TableCell><asp:Button ID ="sftpCancel" runat="server" Style="cursor:pointer"  OnClick="sftpCancel_Click" Text="Cancel" Width="141px" TabIndex="17" /> </asp:TableCell>
                   <asp:TableCell><asp:Button ID ="sftpContinue" runat="server" Style="cursor:pointer"  OnClick="sftpContinue_Click" Text="Continue anyway" Width="141px" TabIndex="18" /> </asp:TableCell>
                   <asp:TableCell><asp:Button ID ="sftpIT" runat="server" Style="cursor:pointer"  OnClick="sftpIT_Click" Text="Send request to IT" Width="141px" TabIndex="19" /></asp:TableCell>
               </asp:TableRow>
           </asp:Table>
           </p>
            </div>
    </form>
    </body>
</html>
