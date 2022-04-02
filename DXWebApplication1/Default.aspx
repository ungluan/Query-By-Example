<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="Default.aspx.cs" Inherits="DXWebApplication1.Default" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <div id="header">
        <asp:Label ID="LabelTitle" runat="server" Text="Nhập tiêu đề báo cáo: " Font-Size="Medium"></asp:Label>
        <asp:TextBox ID="TextBox" runat="server" Width="995px" Font-Size="Medium"></asp:TextBox>    
    </div>
    <div>
        <asp:Panel ID="Panel1" runat="server">
            <asp:Label ID="Label1" runat="server" Text="Chọn database"></asp:Label>
            <asp:DropDownList ID="DropDownListDatabase" runat="server" OnSelectedIndexChanged="DropDownListDatabase_SelectedIndexChanged" AutoPostBack="True">
            </asp:DropDownList>
        </asp:Panel>
    </div>
    <div>
        <asp:Panel ID="Panel2" runat="server">
            <asp:Panel ID="Panel3" runat="server" Width="500px" Height="58px">
                <asp:Label ID="Label2" runat="server" Text="Chọn Table"></asp:Label>
                <asp:CheckBoxList ID="CheckBoxListTable" runat="server" OnSelectedIndexChanged="CheckBoxListTable_SelectedIndexChanged" >
                </asp:CheckBoxList>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="Button" />
                <br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" BackColor="White" BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" CellSpacing="1" GridLines="None" Width="1255px">
                    <Columns>
                        <asp:TemplateField HeaderText="Table">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownListTable2" runat="server">

                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Field">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownListColumn" runat="server">
                                
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownListAction" runat="server">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                    <asp:ListItem Text="Count" Value="Count"></asp:ListItem>
                                    <asp:ListItem Text="Sum" Value="Sum"></asp:ListItem>
                                    <asp:ListItem Text="Min" Value="Min"></asp:ListItem>
                                    <asp:ListItem Text="Max" Value="Max"></asp:ListItem>                                     
                                    <asp:ListItem Text="Avg" Value="Avg"></asp:ListItem>
                                    <asp:ListItem Text="Group by" Value="Group by"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Where" />
                        <asp:BoundField HeaderText="Having" />
                        <asp:TemplateField HeaderText="Sort">
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownListSort" runat="server">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Increasement" Value="ASC"></asp:ListItem>
                                    <asp:ListItem Text="Decreasement" Value="DESC"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Alias" />
                    </Columns>
                    <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" />
                    <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                    <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#594B9C" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#33276A" />
                </asp:GridView>
            </asp:Panel>
        </asp:Panel>
    </div>
</asp:Content>