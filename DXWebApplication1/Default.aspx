<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="Default.aspx.cs" Inherits="DXWebApplication1.Default" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <div id="header">
        <asp:Label ID="LabelTitle" runat="server" Text="Nhập tiêu đề báo cáo: " Font-Bold="true" style="color:#fffffe;margin-top: 16px;margin-bottom: 16px;" Font-Size="Medium"></asp:Label>
        <asp:TextBox ID="TextBox" runat="server" Width="1019px" Font-Size="Medium"  BorderColor="#EFA32A" CssClass="input"></asp:TextBox>    
    </div>
    <div>
        <asp:Label style="color:#fffffe" Font-Bold="true" ID="Label1" runat="server" Text="Chọn database:"></asp:Label>
        <asp:DropDownList ID="DropDownListDatabase" style ="margin-left: 42px;margin-top: 16px;margin-bottom: 16px; padding-left:24px;padding-right:24px; padding-top:8px; padding-bottom:8px " BorderColor="#EFA32A" runat="server" 
            OnSelectedIndexChanged="DropDownListDatabase_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>
    </div>
        <asp:Label ID="Label2" runat="server" Font-Bold="true" style="color:#fffffe;margin-top: 16px;margin-bottom: 16px;" Text="Chọn Table:"></asp:Label>
    <div>
            <asp:CheckBoxList ID="CheckBoxListTable" runat="server" BorderWidth="1" style="margin-top:16px; margin-bottom:16px"
            BorderColor="#abd1c6" CssClass="alignCheckBoxList" CellSpacing="16" CellPadding="16"
                RepeatLayout="Table" RepeatColumns="4" 
                BackColor="#f9bc60" RepeatDirection="Horizontal" 
                OnSelectedIndexChanged="CheckBoxListTable_SelectedIndexChanged" AutoPostBack="True" >
            </asp:CheckBoxList>                
            <asp:TextBox ID="TextBox1" runat="server" style="margin-right: 24px" TextMode="MultiLine"  Rows="8" Width="1176px"  ></asp:TextBox>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" GridLines="None" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" style="margin-right: 24px;margin-top:16px; margin-bottom:16px;" Width="1178px" ForeColor="#333333">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="Field">
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownListColumn" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Two</asp:ListItem>
                                <asp:ListItem>Three</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Select" >
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownListAction" runat="server">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                <asp:ListItem Text="Count" Value="Count"></asp:ListItem>
                                <asp:ListItem Text="Sum" Value="Sum"></asp:ListItem>
                                <asp:ListItem Text="Min" Value="Min"></asp:ListItem>
                                <asp:ListItem Text="Max" Value="Max"></asp:ListItem>
                                <asp:ListItem Text="Avg" Value="Avg"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Where">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxWhere" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Group By">
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownGroupBy" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>True</asp:ListItem>
                                <asp:ListItem>False</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Having">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxHaving" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sort">
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownListSort" runat="server">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Increasement" Value="ASC"></asp:ListItem>
                                <asp:ListItem Text="Decreasement" Value="DESC"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Alias">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxAlias" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
    
    </div>
    <div style="text-align: center; margin-top:16px" >
        <asp:Button ID="Button1" style="margin-right: 32px" CssClass="button" runat="server" OnClick="Button1_Click1" Text="Query" />
        <asp:Button ID="Button2" CssClass="button" runat="server" Text="Create Report" />
    </div>
</asp:Content>