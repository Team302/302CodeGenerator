using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public partial class robot
    {
    public void initialize()
    {
        if (pigeonField == null)
            pigeonField = new pigeon[] { };

        if (mechanismField == null)
            mechanismField = new mechanism[] { };

        if (cameraField == null)
            cameraField = new camera[] { };

        if (roborioField == null)
            roborioField = new roborio[] { };
    }
}

