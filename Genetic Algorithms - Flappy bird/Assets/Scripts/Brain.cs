using UnityEngine;

namespace Assets.Scripts {
    public class Brain : MonoBehaviour {
        public DNA DNA { get; set; }
        public float DistanceTravelled { get; set; }

        [SerializeField] private GameObject _eyes;

        private KnownSituation _currentSituation = KnownSituation.Default;
        private float _timeAlive = 0;
        private Vector3 _startPosition;
        private int _crash = 0;
        private bool _isAlive = true;
        private Rigidbody2D _rigidBody;
        private Collider2D _collider;
    
        public void Start() {
            this._collider = this.GetComponent<Collider2D>();

            this.transform.Translate(Random.Range(-1.5f,1.5f),Random.Range(-1.5f,1.5f),0);
            this._startPosition = this.transform.position;
            this._rigidBody = this.GetComponent<Rigidbody2D>();
        }

        void OnCollisionEnter2D(Collision2D col) {
            if(col.gameObject.tag == "dead" || col.gameObject.tag == "top" || col.gameObject.tag == "bottom" || col.gameObject.tag == "upwall" || col.gameObject.tag == "downwall") {
                this._crash++;
            }

            if (col.gameObject.tag == "bird") {
                Physics2D.IgnoreCollision(this._collider, col.collider);
            }
        }


        void Update() {
            if (!this._isAlive) {
                return;
            }

            Debug.DrawRay(this._eyes.transform.position, this._eyes.transform.forward * 2.0f, Color.red);
            Debug.DrawRay(this._eyes.transform.position, this._eyes.transform.up* 2.0f, Color.red);
            Debug.DrawRay(this._eyes.transform.position, -this._eyes.transform.up* 2.0f, Color.red);

            this._currentSituation = KnownSituation.Default;

            //RaycastHit2D hit = Physics2D.Raycast(this.eyes.transform.position, this.eyes.transform.up, 2.0f);
            //if (hit.collider != null) {
            //    if(hit.collider.gameObject.tag == "top") {
            //        //this._currentSituation = KnownSituation.HitTop;
            //    }
            //}
            //hit = Physics2D.Raycast(this.eyes.transform.position, -this.eyes.transform.up, 2.0f);
            //if (hit.collider != null) {    
            //    if(hit.collider.gameObject.tag == "bottom") {
            //        //this._currentSituation = KnownSituation.HitBottom;
            //    }
            //}
            RaycastHit2D hit = Physics2D.Raycast(this._eyes.transform.position, this._eyes.transform.up, 2.0f);
            if (hit.collider != null) {
                if (hit.collider.gameObject.tag == "upwall") {
                    this._currentSituation = KnownSituation.HitTopWall;
                }
                else if (hit.collider.gameObject.tag == "downwall") {
                    this._currentSituation = KnownSituation.HitBottomWall;
                }
            }
            this._timeAlive += Time.deltaTime;
        }


        void FixedUpdate() {
            if (!this._isAlive) {
                return;
            }

            KnownAction currentAction = this.DNA.SituationMap[this._currentSituation];
            this.ExecuteAction(currentAction);
        }

        private void ExecuteAction(KnownAction action) {
            float verticalForce = 0f;
            float horizontalForce = 1f;

            switch (action) {
                case KnownAction.FlyForward:
                    horizontalForce += 15;
                    break;
                case KnownAction.FlyDown:
                    verticalForce += -175;
                    break;
                case KnownAction.FlyUp:
                    verticalForce += 175;
                    break;
            }

            this._rigidBody.AddForce(this.transform.right * horizontalForce);
            this._rigidBody.AddForce(this.transform.up * verticalForce * 0.1f);
            this.DistanceTravelled = this.transform.position.x - this._startPosition.x;
        }
    }
}

