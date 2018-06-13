using UnityEngine;

namespace Assets {
    public class DNA : MonoBehaviour {
        public float r;
        public float g;
        public float b;

        public float timeBeforeDeath = 0f;
        private bool isDead = false;

        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;



        private void Start() {
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
            this._collider2D = this.GetComponent<Collider2D>();

            this._spriteRenderer.color = new Color(this.r, this.g, this.b);
        }

        private void Update() {
            
        }

        private void OnMouseDown() {
            this.isDead = true;
            this.timeBeforeDeath = PopulationManager.elapsedTime;
            this._spriteRenderer.enabled = false;
            this._collider2D.enabled = false;
        }

    }
}