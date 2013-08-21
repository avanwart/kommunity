<%@ Page Title="" Language="C#" MasterPageFile="~/m/auth/Shared/Main.Master" AutoEventWireup="true" CodeBehind="VideoInput.aspx.cs" Inherits="DasKlub.Web.m.auth.VideoInput" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:HiddenField ID="hfVideoRequestID" runat="server" />
 

    <asp:GridView AutoGenerateColumns="False" DataKeyNames="VideoRequestID" ID="gvwRequestedVideos"
                  runat="server" BackColor="White"
                  BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                  GridLines="Vertical" 
                  onselectedindexchanged="gvwRequestedVideos_SelectedIndexChanged">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="videoRequestID" HeaderText="VideoRequest ID" />
            <asp:BoundField DataField="requestURL" HeaderText="Request URL" />
            <asp:BoundField DataField="createDate" HeaderText="Create Date"  DataFormatString="{0:u}"  />
            
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
                <asp:TextBox ID="txtURL" runat="server" Width="293px"></asp:TextBox>
            </td>
  
            <td>
                <asp:Button ID="btnFetchURL" runat="server" Text="Fetch" 
                            onclick="btnFetchURL_Click" />
            </td>
        </tr>


    </table>


    <asp:Literal ID="litVideo" runat="server"></asp:Literal>
 
     

    <br />
    <br />
     
    <asp:Button ID="btnReject" runat="server" Text="Reject Video" Enabled="false" 
                onclick="btnReject_Click" />
    <br />


    <br />
    <asp:Label ID="lblStatus" runat="server"></asp:Label>
    <br />

    <asp:Label ID="lblVideoID" runat="server"></asp:Label>
    <div>
        <table>
            <tr>
                <td>
                    Video Provider:
                </td>
                <td>
                    <asp:DropDownList ID="ddlVideoProvider" runat="server">
                        <asp:ListItem Value="YT" Text="You Tube"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Video Key:
                </td>
                <td>
                    <asp:TextBox ID="txtVideoKey" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Username:
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Seconds Duration:
                </td>
                <td>
                    <asp:TextBox ID="txtDuration" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Seconds In:
                </td>
                <td>
                    <asp:TextBox ID="txtSecondsIn" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Contest
                </td>
                <td>
                    <asp:DropDownList ID="ddlContest" runat="server"></asp:DropDownList>
                </td>
            </tr>


            <tr>
                <td>
                    Seconds Elapsed End:
                </td>
                <td>
                    <asp:TextBox ID="txtElasedEnd" runat="server"></asp:TextBox>
                </td>
            </tr>
           
            <tr>
                <td>
                    Human Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddlHumanType" runat="server">
                      
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Footage Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddlFootageType" runat="server">
                       
                    </asp:DropDownList>
                </td>
            </tr>
             
            <tr>
                <td>
                    Video Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddlVideoType" runat="server">
   
                    </asp:DropDownList>
                </td>
            </tr>

                     
 
            <tr>
                <td>
                    Volume Level Adjustment:
                </td>
                <td>
                    <asp:DropDownList ID="ddlVolumeLevel" runat="server">
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3">3</asp:ListItem>
                        <asp:ListItem Value="4">4</asp:ListItem>
                        <asp:ListItem Value="5">5</asp:ListItem>
                        <asp:ListItem Value="6">6</asp:ListItem>
                        <asp:ListItem Value="7">7</asp:ListItem>
                        <asp:ListItem Value="8">8</asp:ListItem>
                        <asp:ListItem Value="9">9</asp:ListItem>
                        <asp:ListItem Value="10">10</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Enabled:
                </td>
                <td>
                    <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" />
                </td>
            </tr>
           
            


            <tr>
                <td>
                    :
                </td>
                <td>
                    Up toEmail 5 songs in order of appearance
                </td>
            </tr>
            <tr>
                <td>
                    Artist:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtist1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArtist1_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtist1" runat="server" Width="255px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Song:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtistSongs1" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtistSong1" runat="server" Width="252px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Album:
                </td>
                <td>
                    <asp:DropDownList ID="ddlAlbumName1" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtAlbumName1" runat="server" Width="252px"></asp:TextBox>
                    #<asp:TextBox ID="txtTrackNumber1" runat="server" Width="125px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    :
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Artist:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtist2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArtist2_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtist2" runat="server" Width="255px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Song:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtistSongs2" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtistSong2" runat="server" Width="252px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Album:
                </td>
                <td>
                    <asp:DropDownList ID="ddlAlbumName2" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtAlbumName2" runat="server" Width="252px"></asp:TextBox>
                    #<asp:TextBox ID="txtTrackNumber2" runat="server" Width="125px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    :
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Artist:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtist3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArtist3_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtist3" runat="server" Width="255px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Song:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtistSongs3" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtistSong3" runat="server" Width="252px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Album:
                </td>
                <td>
                    <asp:DropDownList ID="ddlAlbumName3" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtAlbumName3" runat="server" Width="252px"></asp:TextBox>
                    #<asp:TextBox ID="txtTrackNumber3" runat="server" Width="125px"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <td>
                    :
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Artist:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtist4" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArtist4_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtist4" runat="server" Width="255px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Song:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtistSongs4" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtistSong4" runat="server" Width="252px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Album:
                </td>
                <td>
                    <asp:DropDownList ID="ddlAlbumName4" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtAlbumName4" runat="server" Width="252px"></asp:TextBox>
                    #<asp:TextBox ID="txtTrackNumber4" runat="server" Width="125px"></asp:TextBox>
                </td>
            </tr>


















            <tr>
                <td>
                    :
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Artist:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtist5" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArtist5_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtist5" runat="server" Width="255px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Song:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtistSongs5" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtistSong5" runat="server" Width="252px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Album:
                </td>
                <td>
                    <asp:DropDownList ID="ddlAlbumName5" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtAlbumName5" runat="server" Width="252px"></asp:TextBox>
                    #<asp:TextBox ID="txtTrackNumber5" runat="server" Width="125px"></asp:TextBox>
                </td>
            </tr>









            <tr>
                <td>
                    :
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Artist:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtist6" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArtist6_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtist6" runat="server" Width="266px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Song:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtistSongs6" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtArtistSong6" runat="server" Width="262px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Album:
                </td>
                <td>
                    <asp:DropDownList ID="ddlAlbumName6" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtAlbumName6" runat="server" Width="262px"></asp:TextBox>
                    #<asp:TextBox ID="txtTrackNumber6" runat="server" Width="126px"></asp:TextBox>
                </td>
            </tr>






















            <tr>
                <td>
                </td>
                <td>
                    <asp:Button CssClass="btn btn-success" ID="btnSubmit" runat="server" Text="Set Video Entry" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    </div>
 


</asp:Content>