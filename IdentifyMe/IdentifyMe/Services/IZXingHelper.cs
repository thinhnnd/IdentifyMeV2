using System.Collections.Generic;
using ZXing.Mobile;

namespace IdentifyMe.Services
{
    public interface IZXingHelper
    {
        CameraResolution CameraResolutionSelectorDelegateImplementation(List<CameraResolution> availableresolutions);
    }
}
