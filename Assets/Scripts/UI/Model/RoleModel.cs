using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoleModel : Singleton<RoleModel>
{
    private RoleData roleData;

    public RoleInfo GetRole(int id)
    {
        if (roleData == null)
        {
            roleData = JsonTool.LoadJson<RoleData>("roleData");
        }
        return roleData.roles[id-1];
    }
}
