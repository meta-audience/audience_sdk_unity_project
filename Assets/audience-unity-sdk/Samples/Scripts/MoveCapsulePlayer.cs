using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudienceSDK.Sample
{
    public class MoveCapsulePlayer : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 5f;

        private Rigidbody _capsuleRigidbody = null;


        // Start is called before the first frame update
        private void Start()
        {
            this._capsuleRigidbody = this.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            Vector3 moveForward = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (moveForward.magnitude > 0)
            {
                this._capsuleRigidbody.isKinematic = false;
                this._capsuleRigidbody.MovePosition(transform.position + moveForward * Time.deltaTime * this._speed);
                this.transform.rotation = Quaternion.LookRotation(moveForward, Vector3.up);
            }
            else
            {
                this._capsuleRigidbody.isKinematic = true;
            }
        }
    }
}