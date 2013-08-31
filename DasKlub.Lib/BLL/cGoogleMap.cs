//   Google Maps User Control for ASP.Net version 1.0:
//   ========================
//   Copyright (C) 2008  Shabdar Ghata 
//   Email : ghata2002@gmail.com
//   URL : http://www.shabdar.org

//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.

//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.

//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.

//   This program comes with ABSOLUTELY NO WARRANTY.

using System;
using System.Collections;
using System.Drawing;
using System.Web;

namespace DasKlub.Lib.BLL
{
    /// <summary>
    ///     Summary description for cGoogleMap
    /// </summary>
    [Serializable]
    public class GoogleObject
    {
        private string _apikey = "";
        private string _apiversion = "2";
        private GooglePoint _centerpoint = new GooglePoint();
        private GooglePoints _gpoints = new GooglePoints();
        private GooglePolygons _gpolygons = new GooglePolygons();
        private GooglePolylines _gpolylines = new GooglePolylines();
        private string _height = "400px";
        private string _maptype = "";
        private bool _recentermap;
        private bool _showmaptypescontrol = true;
        private bool _showtraffic;
        private bool _showzoomcontrol = true;
        private string _width = "500px";
        private int _zoomlevel = 3;

        public GoogleObject()
        {
        }

        public GoogleObject(GoogleObject prev)
        {
            Points = GooglePoints.CloneMe(prev.Points);
            Polylines = GooglePolylines.CloneMe(prev.Polylines);
            Polygons = GooglePolygons.CloneMe(prev.Polygons);
            ZoomLevel = prev.ZoomLevel;
            ShowZoomControl = prev.ShowZoomControl;
            ShowMapTypesControl = prev.ShowMapTypesControl;
            Width = prev.Width;
            Height = prev.Height;
            MapType = prev.MapType;
            APIKey = prev.APIKey;
            ShowTraffic = prev.ShowTraffic;
            RecenterMap = prev.RecenterMap;
        }

        public GooglePoints Points
        {
            get { return _gpoints; }
            set { _gpoints = value; }
        }

        public GooglePolylines Polylines
        {
            get { return _gpolylines; }
            set { _gpolylines = value; }
        }

        public GooglePolygons Polygons
        {
            get { return _gpolygons; }
            set { _gpolygons = value; }
        }

        public GooglePoint CenterPoint
        {
            get { return _centerpoint; }
            set { _centerpoint = value; }
        }

        public int ZoomLevel
        {
            get { return _zoomlevel; }
            set { _zoomlevel = value; }
        }

        public bool ShowZoomControl
        {
            get { return _showzoomcontrol; }
            set { _showzoomcontrol = value; }
        }

        public bool RecenterMap
        {
            get { return _recentermap; }
            set { _recentermap = value; }
        }

        public bool ShowTraffic
        {
            get { return _showtraffic; }
            set { _showtraffic = value; }
        }

        public bool ShowMapTypesControl
        {
            get { return _showmaptypescontrol; }
            set { _showmaptypescontrol = value; }
        }

        public string Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public string Height
        {
            get { return _height; }
            set { _height = value; }
        }


        private string MapType
        {
            get { return _maptype; }
            set { _maptype = value; }
        }

        private string APIKey
        {
            get { return _apikey; }
            set { _apikey = value; }
        }

        public string APIVersion
        {
            get { return _apiversion; }
            set { _apiversion = value; }
        }
    }

    public class GooglePoint
    {
        private string _icon = "";
        private int _iconimageheight = 32;
        private int _iconimagewidth = 32;
        private string _id = "";
        private string _infohtml = "";
        private string _tooltip = "";

        public GooglePoint()
        {
        }

        public GooglePoint(string pID, double plat, double plon, string picon, string pinfohtml, string pTooltipText,
                           bool pDraggable)
        {
            ID = pID;
            Latitude = plat;
            Longitude = plon;
            IconImage = picon;
            InfoHTML = pinfohtml;
            ToolTip = pTooltipText;
            Draggable = pDraggable;
        }

        public string ID
        {
            get { return _id; }
            private set { _id = value; }
        }

        public string IconImage
        {
            get { return _icon; }
            
            private set
            {
                var sIconImage = value;
                if (sIconImage == "")
                    return;
                var imageIconPhysicalPath = cCommon.GetLocalPath() + sIconImage.Replace("/", "\\");

                using (Image img = Image.FromFile(imageIconPhysicalPath))
                {
                    IconImageWidth = img.Width;
                    IconImageHeight = img.Height;
                }
                _icon = cCommon.GetHttpURL() + sIconImage;


                _icon = value;
            }
        }

        private int IconImageWidth
        {
            get { return _iconimagewidth; }
            set { _iconimagewidth = value; }
        }

        public bool Draggable { get; set; }

        private int IconImageHeight
        {
            get { return _iconimageheight; }
            set { _iconimageheight = value; }
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string InfoHTML
        {
            get { return _infohtml; }
            set { _infohtml = value; }
        }

        public string ToolTip
        {
            get { return _tooltip; }
            set { _tooltip = value; }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var p = obj as GooglePoint;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (InfoHTML == p.InfoHTML) && (IconImage == p.IconImage) && (p.ID == ID) && (p.Latitude == Latitude) &&
                   (p.Longitude == Longitude);
        }
    }

    public class GooglePoints : CollectionBase
    {
        private GooglePoint this[int pIndex]
        {
            get { return (GooglePoint) List[pIndex]; }
        }

        public static GooglePoints CloneMe(GooglePoints prev)
        {
            var p = new GooglePoints();
            for (var i = 0; i < prev.Count; i++)
            {
                p.Add(new GooglePoint(prev[i].ID, prev[i].Latitude, prev[i].Longitude, prev[i].IconImage,
                                      prev[i].InfoHTML, prev[i].ToolTip, prev[i].Draggable));
            }
            return p;
        }

        private void Add(GooglePoint pPoint)
        {
            List.Add(pPoint);
        }

        public void Remove(int pIndex)
        {
            RemoveAt(pIndex);
        }

        public void Remove(string pID)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].ID == pID)
                {
                    List.RemoveAt(i);
                    return;
                }
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var p = obj as GooglePoints;
            if (p == null)
            {
                return false;
            }

            if (p.Count != Count)
                return false;


            for (int i = 0; i < p.Count; i++)
            {
                if (!this[i].Equals(p[i]))
                    return false;
            }
            // Return true if the fields match:
            return true;
        }
    }

    public class GooglePolyline
    {
        private bool Equals(GooglePolyline other)
        {
            return string.Equals(_colorcode, other._colorcode) && Equals(_gpoints, other._gpoints) && string.Equals(_id, other._id) && string.Equals(_linestatus, other._linestatus) 
                && _width == other._width && Geodesic.Equals(other.Geodesic);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_colorcode != null ? _colorcode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_gpoints != null ? _gpoints.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_id != null ? _id.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_linestatus != null ? _linestatus.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ _width;
                hashCode = (hashCode*397) ^ Geodesic.GetHashCode();
                return hashCode;
            }
        }

        private string _colorcode = "#66FF00";
        private GooglePoints _gpoints = new GooglePoints();
        private string _id = "";
        private string _linestatus = ""; //N-New, D-Deleted, C-Changed, ''-No Action
        private int _width = 10;

        public string LineStatus
        {
            get { return _linestatus; }
            set { _linestatus = value; }
        }

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public GooglePoints Points
        {
            get { return _gpoints; }
            set { _gpoints = value; }
        }

        public string ColorCode
        {
            get { return _colorcode; }
            set { _colorcode = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public bool Geodesic { get; set; }

        public override bool Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GooglePolyline) obj);
        }
    }

    public class GooglePolylines : CollectionBase
    {
        public GooglePolyline this[int pIndex]
        {
            get { return (GooglePolyline) List[pIndex]; }
            set { List[pIndex] = value; }
        }

        public static GooglePolylines CloneMe(GooglePolylines prev)
        {
            var p = new GooglePolylines();
            for (int i = 0; i < prev.Count; i++)
            {
                var GPL = new GooglePolyline();
                GPL.ColorCode = prev[i].ColorCode;
                GPL.Geodesic = prev[i].Geodesic;
                GPL.ID = prev[i].ID;
                GPL.Points = GooglePoints.CloneMe(prev[i].Points);
                GPL.Width = prev[i].Width;
                p.Add(GPL);
            }
            return p;
        }

        public void Add(GooglePolyline pPolyline)
        {
            List.Add(pPolyline);
        }

        public void Remove(int pIndex)
        {
            RemoveAt(pIndex);
        }

        public void Remove(string pID)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].ID == pID)
                {
                    List.RemoveAt(i);
                    return;
                }
            }
        }
    }


    public class cCommon
    {
        public static Random random = new Random();

        public static string GetHttpURL()
        {
            string[] s = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] {'/'});
            string path = s[0] + "/";
            for (int i = 1; i < s.Length - 1; i++)
            {
                path = path + s[i] + "/";
            }
            return path;
        }

        public static string GetLocalPath()
        {
            string[] s = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] {'/'});
            string PageName = s[s.Length - 1];
            s = HttpContext.Current.Request.MapPath(PageName).Split(new[] {'\\'});
            string path = s[0] + "\\";
            for (int i = 1; i < s.Length - 1; i++)
            {
                path = path + s[i] + "\\";
            }
            return path;
        }

        public static decimal RandomNumber(decimal min, decimal max)
        {
            decimal Fractions = 10000000;
            var iMin = (int) GetIntegerPart(min*Fractions);
            var iMax = (int) GetIntegerPart(max*Fractions);
            int iRand = random.Next(iMin, iMax);

            decimal dRand = iRand;
            dRand = dRand/Fractions;

            return dRand;
        }


        public static decimal GetIntegerPart(decimal source)
        {
            return decimal.Parse(source.ToString("#.00"));
        }
    }

    public class GooglePolygon
    {
        private bool Equals(GooglePolygon other)
        {
            return string.Equals(_fillcolor, other._fillcolor) && _fillopacity.Equals(other._fillopacity) &&
                Equals(_gpoints, other._gpoints) && string.Equals(_id, other._id) && string.Equals(_status, other._status) && string.Equals(_strokecolor, other._strokecolor) && _strokeopacity.Equals(other._strokeopacity) && _strokeweight == other._strokeweight;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (_fillcolor != null ? _fillcolor.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ _fillopacity.GetHashCode();
                hashCode = (hashCode*397) ^ (_gpoints != null ? _gpoints.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_id != null ? _id.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_status != null ? _status.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_strokecolor != null ? _strokecolor.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ _strokeopacity.GetHashCode();
                hashCode = (hashCode*397) ^ _strokeweight;
                return hashCode;
            }
        }

        private string _fillcolor = "#66FF00";
        private double _fillopacity = 0.2;
        private GooglePoints _gpoints = new GooglePoints();
        private string _id = "";
        private string _status = ""; //N-New, D-Deleted, C-Changed, ''-No Action
        private string _strokecolor = "#0000FF";
        private double _strokeopacity = 1;
        private int _strokeweight = 10;

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public GooglePoints Points
        {
            get { return _gpoints; }
            set { _gpoints = value; }
        }

        public string StrokeColor
        {
            get { return _strokecolor; }
            set { _strokecolor = value; }
        }

        public string FillColor
        {
            get { return _fillcolor; }
            set { _fillcolor = value; }
        }

        public int StrokeWeight
        {
            get { return _strokeweight; }
            set { _strokeweight = value; }
        }

        public double StrokeOpacity
        {
            get { return _strokeopacity; }
            set { _strokeopacity = value; }
        }

        public double FillOpacity
        {
            get { return _fillopacity; }
            set { _fillopacity = value; }
        }

        public override bool Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GooglePolygon) obj);
        }
    }

    public class GooglePolygons : CollectionBase
    {
        public GooglePolygon this[int pIndex]
        {
            get { return (GooglePolygon) List[pIndex]; }
            set { List[pIndex] = value; }
        }

        public GooglePolygon this[string pID]
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i].ID == pID)
                    {
                        return (GooglePolygon) List[i];
                    }
                }
                return null;
            }
            set
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i].ID == pID)
                    {
                        List[i] = value;
                    }
                }
            }
        }

        public static GooglePolygons CloneMe(GooglePolygons prev)
        {
            var p = new GooglePolygons();
            for (int i = 0; i < prev.Count; i++)
            {
                var gpl = new GooglePolygon();
                gpl.FillColor = prev[i].FillColor;
                gpl.FillOpacity = prev[i].FillOpacity;
                gpl.ID = prev[i].ID;
                gpl.Status = prev[i].Status;
                gpl.StrokeColor = prev[i].StrokeColor;
                gpl.StrokeOpacity = prev[i].StrokeOpacity;
                gpl.StrokeWeight = prev[i].StrokeWeight;
                gpl.Points = GooglePoints.CloneMe(prev[i].Points);
                p.Add(gpl);
            }
            return p;
        }

        private void Add(GooglePolygon pPolygon)
        {
            List.Add(pPolygon);
        }
    }
}