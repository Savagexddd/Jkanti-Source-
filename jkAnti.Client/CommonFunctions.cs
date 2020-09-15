using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jkAnti.Client
{
    class CommonFunctions
    {

        public static void RequestAndDelete(int objHandle, bool detach)
        {
            if (!API.DoesEntityExist(objHandle) || !CommonFunctions.RequestNetworkControl(objHandle) || !API.DoesEntityExist(objHandle))
                return;
            if (detach)
                API.DetachEntity(objHandle, false, false);
            API.SetEntityCollision(objHandle, false, false);
            API.SetEntityAlpha(objHandle, 0, 1);
            API.SetEntityAsNoLongerNeeded(ref objHandle);
            if (API.IsEntityAPed(objHandle))
                API.DeletePed(ref objHandle);
            else if (API.IsEntityAVehicle(objHandle))
                API.DeleteVehicle(ref objHandle);
            else if (API.IsEntityAnObject(objHandle))
            {
                API.DeleteObject(ref objHandle);
            }
            else
            {
                if (!API.IsAnEntity(objHandle))
                    return;
                API.DeleteEntity(ref objHandle);
            }
        }


        public static bool RequestNetworkControl(int objHandle)
        {
            int gameTimer = API.GetGameTimer();
            API.NetworkRequestControlOfEntity(objHandle);
            while (!API.NetworkHasControlOfEntity(objHandle) && API.GetGameTimer() - gameTimer > 250)
                API.NetworkRequestControlOfEntity(objHandle);
            return API.NetworkHasControlOfEntity(objHandle);
        }

    }
}
