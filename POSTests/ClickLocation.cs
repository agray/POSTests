using System.Windows;

namespace POSTests {
    public class ClickLocation {
        private string _name;
        private Point _point;

        public ClickLocation(string name, double x, double y) {
            _name = name;
            _point = new Point(x, y);
        }

        public Point point {
            get {
                return _point;
            }

            set {
                _point = value;
            }
        }

        public string name {
            get {
                return _name;
            }

            set {
                _name = value;
            }
        }
    }
}