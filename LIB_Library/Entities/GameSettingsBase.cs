using Core.Entities;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Entities
{
    public class GameSettingsBase : NotifyerPropertyChangedBase, ICloneable
    {
        public virtual object Clone()
        {
            string objectString = XmlSerializerBase.SerializeObjectToString(this);
            return XmlSerializerBase.DeserializeObjectFromString(objectString, this.GetType());
            //return this.MemberwiseClone();
        }
    }
}
