using UnityEngine;

namespace Diguifi.wheelsManager
{
    public class WheelsManager : MonoBehaviour
    {

        public WheelCollider[] wheelTypes;

        WheelCollider wc;
        RaycastHit hit;

        public int currentWheel = 0;

        private void Start()
        {
            wc = GetComponent<WheelCollider>();
        }

        private void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, -transform.up, out hit, 2f))
            {
                if (hit.collider.tag == "Road")
                {
                    if (currentWheel != 0)
                    {
                        currentWheel = 0;
                        wc.sidewaysFriction = wheelTypes[0].sidewaysFriction;
                    }
                }
                if (hit.collider.tag == "Grass")
                {
                    if (currentWheel != 1)
                    {
                        currentWheel = 1;
                        wc.sidewaysFriction = wheelTypes[1].sidewaysFriction;
                    }
                }
            }
        }

    }
}
