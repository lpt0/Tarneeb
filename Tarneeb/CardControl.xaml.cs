using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TarneebClasses;

/**
 * @author  Andrew Kuo, 
 * @date    2021-03-10
 */
namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for CardControl.xaml
    /// </summary>
    public partial class CardControl : UserControl
    {
        /// <summary>
        /// Whether the Card should be rendered facedown.
        /// </summary>
        public bool IsFaceDown
        {
            get; 
            set;
        }

        /// <summary>
        /// Card represented.
        /// </summary>
        private Card card;

        /// <summary>
        /// Card accessor/mutator.
        /// </summary>
        public Card Card
        {
            get { return card; }
            set
            {
                card = value; 
            }
        }

        /// <summary>
        /// Changes the resource displayed by the CardControl.
        /// </summary>
        public Object ImageResource
        {
            get;
            set;
        }

        /// <summary>
        /// Card Constructor.
        /// </summary>
        public CardControl(Card aCard, bool faceDown=false)
        {
            this.IsFaceDown = faceDown;
            this.Card = aCard;
            InitializeComponent();
        }

        /// <summary>
        /// Load the Card resources one the CardControl is loaded.
        /// </summary>
        private void CardControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.CardControlButton.Content = CardToResource(this.Card);
        }


        /// <summary>
        /// Converts a given Card class to its appropriate Image Object.
        /// </summary>
        /// <param name="aCard">A Card Class</param>
        /// <returns>Image Object</returns>
        public static System.Windows.Controls.Image CardToResource(Card aCard)
        {
            //// Going from Bitmap to Bitmap Source.
            //var aBitmap = Tarneeb.Properties.Resources.back;
            //var hBitmap = aBitmap.GetHbitmap();
            //var bitmapSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            //// Attaching Bitmap Source to a Image.
            //var aImage = new System.Windows.Controls.Image();
            //aImage.Source = bitmapSrc;

            // The resource name.
            var resourceName = "";
            // The Bitmap returned from the Resources.
            System.Drawing.Bitmap aBitmap;

            // If no Card is provided, turn it into a double back card. 
            if (aCard is null)
            {
                resourceName = "back";
            }
            else
            { 
                // Determine the suit.
                switch (aCard.Suit)
                {
                    case Enums.CardSuit.Club: resourceName += "c_"; break;
                    case Enums.CardSuit.Diamond: resourceName += "d_"; break;
                    case Enums.CardSuit.Heart: resourceName += "h_"; break;
                    case Enums.CardSuit.Spades: resourceName += "s_"; break;
                }

                // Determine the number.
                switch (aCard.Number)
                {
                    case Enums.CardNumber.Ace: resourceName += "a"; break;
                    case Enums.CardNumber.Two: resourceName += "2"; break;
                    case Enums.CardNumber.Three: resourceName += "3"; break;
                    case Enums.CardNumber.Four: resourceName += "4"; break;
                    case Enums.CardNumber.Five: resourceName += "5"; break;
                    case Enums.CardNumber.Six: resourceName += "6"; break;
                    case Enums.CardNumber.Seven: resourceName += "7"; break;
                    case Enums.CardNumber.Eight: resourceName += "8"; break;
                    case Enums.CardNumber.Nine: resourceName += "9"; break;
                    case Enums.CardNumber.Ten: resourceName += "10"; break;
                    case Enums.CardNumber.Jack: resourceName += "j"; break;
                    case Enums.CardNumber.Queen: resourceName += "q"; break;
                    case Enums.CardNumber.King: resourceName += "k"; break;
                }
            }

            // Attempt to find the the object in the Resources using the resourse name.
            try
            {
                //aBitmap = ASSETS.GetObject(resourceName) as Bitmap;
                aBitmap = (Bitmap)Tarneeb.Properties.Resources.ResourceManager.GetObject(resourceName);
            }
            // Any error, default to double deck card.
            // We can throw errors but it fine for now.
            catch (Exception)
            {
                aBitmap = Tarneeb.Properties.Resources.back;
            }

            // Convert and attaching Bitmap as Source to a Image, and return the final image.
            return new System.Windows.Controls.Image()
            { 
                Source = System.Windows.Interop.Imaging
                    .CreateBitmapSourceFromHBitmap(
                        aBitmap.GetHbitmap(), 
                        IntPtr.Zero, 
                        Int32Rect.Empty, 
                        BitmapSizeOptions.FromEmptyOptions()
                    ) 
            };
        }

        /// <summary>
        /// Throwable/invokeable click event.
        /// </summary>
        public event RoutedEventHandler Click;

        /// <summary>
        ///  Invokes the Click invent if the CardControl received a click.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }

    }
}
