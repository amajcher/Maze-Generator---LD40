using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class SelfRotation : MonoBehaviour
    {
        public float SelfRotateSpeed = 25f;

        public void Update()
        {
            transform.Rotate(-transform.up, SelfRotateSpeed * Time.deltaTime);
            
        }
    }
}
