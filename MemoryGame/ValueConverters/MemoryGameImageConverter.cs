using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using static MemoryGame.Common.Constants;

namespace MemoryGame.ValueConverters
{
    public class MemoryGameImageConverter : IValueConverter
    {
        private string cardBackPath = Path.GetFullPath("Images\\back.png");
        private string cardAlienPath = Path.GetFullPath("Images\\alien.png");
        private string cardBugPath = Path.GetFullPath("Images\\bug.png");
        private string cardDuckPath = Path.GetFullPath("Images\\duck.png");
        private string cardRocketPath = Path.GetFullPath("Images\\rocket.png");
        private string cardSpaceshipPath = Path.GetFullPath("Images\\spaceship.png");
        private string cardTiktacPath = Path.GetFullPath("Images\\tiktac.png");
        /// <summary>
        /// </summary>
        /// <param name="value">type => bool (the card is turned)</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">The type of card</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage result = null;
            string ImagePath = cardBackPath;
            if(value != null && value is bool isTurned)
            {
                if (!isTurned)
                {
                    ImagePath = cardBackPath;
                }
                else
                {
                    if(parameter != null && parameter is CardTypes cardType)
                    {
                        switch(cardType)
                        {
                            case CardTypes.alien:
                                ImagePath = cardAlienPath;
                                break;
                            case CardTypes.bug:
                                ImagePath = cardBugPath;
                                break;
                            case CardTypes.duck:
                                ImagePath = cardDuckPath;
                                break;
                            case CardTypes.rocket:
                                ImagePath = cardRocketPath;
                                break;
                            case CardTypes.spaceship:
                                ImagePath = cardSpaceshipPath;
                                break;
                            case CardTypes.tiktac:
                                ImagePath = cardTiktacPath;
                                break;
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(parameter));
                    }
                }
            }
            if (!String.IsNullOrEmpty(ImagePath))
            {
                result = new BitmapImage(new Uri(ImagePath));
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
