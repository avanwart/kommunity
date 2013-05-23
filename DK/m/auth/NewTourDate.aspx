<%@ Page Title="" Language="C#" MasterPageFile="~/m/auth/Shared/Main.Master" AutoEventWireup="true"
         CodeBehind="NewTourDate.aspx.cs" Inherits="DasKlub.Web.Web.m.auth.NewTourDate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div>
        <h2>
            Event</h2>

        <asp:HiddenField ID="hfEventID" runat="server" />


        <script type="text/javascript">
            function confirmSubmit() {

                var agree = confirm("Do you really want to delete this event?");

                if (agree)
                    return true;

                else
                    return false;

            }

        </script>



        <asp:GridView AutoGenerateColumns="False" DataKeyNames="eventID" ID="gvwEvents" runat="server"
                      BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                      onselectedindexchanged="gvwAllEvents_SelectedIndexChanged" 
                      CellPadding="4" ForeColor="Black" GridLines="Vertical" AllowPaging="True">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="eventID" HeaderText="Event ID" />
                <asp:BoundField DataField="name" HeaderText="name" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnDeleteEvent" Text="delete" runat="server" 
                                    OnClick="btnDeleteEvent_Click"
                                    OnClientClick=" return confirmSubmit() " />
                    </ItemTemplate>
                </asp:TemplateField>







            </Columns>

            <FooterStyle BackColor="#CCCC99" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <RowStyle BackColor="#F7F7DE" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#FBFBF2" />
            <SortedAscendingHeaderStyle BackColor="#848384" />
            <SortedDescendingCellStyle BackColor="#EAEAD3" />
            <SortedDescendingHeaderStyle BackColor="#575357" />

        </asp:GridView>








        <table>
            <tr>
                <td>
                    Venue:
                </td>
                <td>
                    <asp:DropDownList ID="ddlVenues" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Local Time:
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server">
                        <asp:ListItem>2011</asp:ListItem>
                        <asp:ListItem>2012</asp:ListItem>
                        <asp:ListItem>2013</asp:ListItem>
                    </asp:DropDownList>
                    -
                    <asp:DropDownList ID="ddlMonth" runat="server">
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                    </asp:DropDownList>
                    -
                    <asp:DropDownList ID="ddlDay" runat="server">
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>26</asp:ListItem>
                        <asp:ListItem>27</asp:ListItem>
                        <asp:ListItem>28</asp:ListItem>
                        <asp:ListItem>29</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>31</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Cycle:
                </td>
                <td>
                    <asp:DropDownList ID="ddlEventCycle" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Is Reoccuring:
                </td>
                <td>
                    <asp:CheckBox ID="chkIsReoccuring" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Is Enabled:
                </td>
                <td>
                    <asp:CheckBox ID="chkIsEnabled" runat="server" />
                </td>
            </tr>

            <tr>
                <td>
                    Name:
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Width="372px"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <td>
                    Ticket URL:
                </td>
                <td>
                    <asp:TextBox ID="txtTicketURL" runat="server" Width="372px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    RSVP URL:
                </td>
                <td>
                    <asp:TextBox ID="txtRSVPURL" runat="server" Width="372px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Event Detail URL:
                </td>
                <td>
                    <asp:TextBox ID="txtEventDetailURL" runat="server" Width="372px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Notes:
                </td>
                <td>
                    <asp:TextBox TextMode="MultiLine" ID="txtNotes" runat="server" Height="126px" Width="389px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Update" OnClick="btnSubmit_Click" />

                    <asp:Button ID="btnNewEvent" runat="server" Text="Create New Event" 
                                onclick="btnNewEvent_Click"   />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>