/* site.js — corrected bubble code (paste over existing file) */

document.addEventListener('DOMContentLoaded', function () {
    // nav & scroll highlight (unchanged)
    const sections = document.querySelectorAll('section[id]');
    const navLinks = document.querySelectorAll('.nav-link');

    navLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const targetId = this.getAttribute('href');
            const targetSection = document.querySelector(targetId);
            if (targetSection) {
                window.scrollTo({ top: targetSection.offsetTop - 80, behavior: 'smooth' });
            }
        });
    });

    function updateActiveNav() {
        let currentSectionId = '';
        sections.forEach(section => {
            const sectionTop = section.offsetTop - 120;
            const sectionHeight = section.offsetHeight;
            if (window.scrollY >= sectionTop && window.scrollY < sectionTop + sectionHeight) {
                currentSectionId = section.getAttribute('id');
            }
        });
        navLinks.forEach(link => {
            link.classList.remove('active');
            if (link.getAttribute('href') === `#${currentSectionId}`) link.classList.add('active');
        });
    }
    window.addEventListener('scroll', updateActiveNav);
    updateActiveNav();
});


/* ---------------- BUBBLES ---------------- */
(function () {
    function ready(fn) {
        if (document.readyState !== 'loading') fn();
        else document.addEventListener('DOMContentLoaded', fn);
    }

    ready(function () {
        const root = document.getElementById('bubbles-root');
        if (!root) {
            console.warn('bubbles: #bubbles-root not found — skipping animation.');
            return;
        }

        const IMAGE_PATH = '/images/bubbleFloat.png';

        const BUBBLES = [
            // right-zone (majority)
            { id: 'b1', zone: 'right', w: 420, h: 280, clip: 'polygon(6% 42%, 42% 22%, 68% 32%, 84% 58%, 48% 82%, 22% 68%)', bgPos: '70% 10%', bgSize: '200% auto', speed: 0.28, spin: 5 },
            { id: 'b2', zone: 'right', w: 360, h: 260, clip: 'polygon(8% 60%, 36% 44%, 60% 52%, 72% 82%, 36% 94%, 12% 80%)', bgPos: '78% 22%', bgSize: '200% auto', speed: 0.34, spin: -8 },
            { id: 'b3', zone: 'right', w: 300, h: 220, clip: 'polygon(48% 14%, 74% 6%, 96% 20%, 88% 46%, 60% 36%)', bgPos: '88% 6%', bgSize: '220% auto', speed: 0.22, spin: 4 },
            { id: 'b4', zone: 'right', w: 300, h: 200, clip: 'polygon(66% 42%, 86% 34%, 98% 52%, 78% 76%, 58% 62%)', bgPos: '92% 18%', bgSize: '220% auto', speed: 0.40, spin: 10 },
            { id: 'b5', zone: 'right', w: 380, h: 220, clip: 'polygon(20% 10%, 48% 4%, 76% 22%, 86% 46%, 52% 66%, 14% 44%)', bgPos: '60% 2%', bgSize: '210% auto', speed: 0.30, spin: -6 },

            // medium
            { id: 'b6', zone: 'right', w: 220, h: 180, clip: 'circle(45% at 40% 50%)', bgPos: '64% 36%', bgSize: '200% auto', speed: 0.36, spin: 7 },
            { id: 'b7', zone: 'right', w: 210, h: 140, clip: 'polygon(40% 10%, 70% 4%, 92% 20%, 86% 44%, 62% 38%)', bgPos: '76% 8%', bgSize: '200% auto', speed: 0.26, spin: -9 },
            { id: 'b8', zone: 'right', w: 240, h: 160, clip: 'polygon(12% 12%, 38% 8%, 68% 20%, 76% 44%, 40% 48%)', bgPos: '72% 6%', bgSize: '200% auto', speed: 0.30, spin: 6 },

            // fill
            { id: 'b9', zone: 'right', w: 180, h: 160, clip: 'circle(50% at 52% 28%)', bgPos: '82% 2%', bgSize: '200% auto', speed: 0.44, spin: 10 },
            { id: 'b10', zone: 'right', w: 160, h: 120, clip: 'polygon(6% 10%, 38% 6%, 62% 18%, 56% 42%, 20% 40%)', bgPos: '70% 34%', bgSize: '200% auto', speed: 0.40, spin: -5 },
            { id: 'b11', zone: 'right', w: 120, h: 120, clip: 'circle(50% at 50% 50%)', bgPos: '88% 4%', bgSize: '200% auto', speed: 0.48, spin: 12 },
            { id: 'b12', zone: 'right', w: 140, h: 100, clip: 'polygon(6% 10%, 38% 6%, 62% 18%, 56% 42%, 20% 40%)', bgPos: '68% 40%', bgSize: '200% auto', speed: 0.38, spin: -6 },

            // global roamers
            { id: 'g1', zone: 'global', w: 180, h: 140, clip: 'circle(48% at 54% 52%)', bgPos: '50% 50%', bgSize: '180% auto', speed: 0.36, spin: 10 },
            { id: 'g2', zone: 'global', w: 160, h: 120, clip: 'polygon(6% 10%, 38% 6%, 62% 18%, 56% 42%, 20% 40%)', bgPos: '30% 60%', bgSize: '180% auto', speed: 0.42, spin: -8 },
            { id: 'g3', zone: 'global', w: 120, h: 100, clip: 'circle(45% at 48% 52%)', bgPos: '20% 30%', bgSize: '180% auto', speed: 0.44, spin: 6 }
        ];

        function randomVelocity() {
            return (Math.random() > 0.5 ? 1 : -1) * (0.06 + Math.random() * 0.16);
        }

        function createBubble(cfg, i) {
            const el = document.createElement('div');
            el.className = 'bubble';
            el.id = cfg.id;
            el.style.width = cfg.w + 'px';
            el.style.height = cfg.h + 'px';
            el.style.clipPath = cfg.clip;
            el.style.webkitClipPath = cfg.clip;
            el.style.backgroundImage = `url('${IMAGE_PATH}')`;
            el.style.backgroundPosition = cfg.bgPos || '0 0';
            el.style.backgroundSize = cfg.bgSize || `${cfg.w * 2}px auto`;

            if (cfg.zone === 'right') {
                // place inside right container
                el.style.position = 'absolute';
                // ensure it's visible but behind .hero-content (which is z-index:200)
                el.style.zIndex = 80;
                root.appendChild(el);

                const rect = root.getBoundingClientRect();
                const x = Math.random() * Math.max(0, rect.width - cfg.w);
                const y = Math.random() * Math.max(0, rect.height - cfg.h);

                el._state = { x, y, vx: randomVelocity(), vy: randomVelocity(), last: null, rot: Math.random() * 360, spin: cfg.spin || 6, speedMul: cfg.speed || 0.35, w: cfg.w, h: cfg.h, zone: 'right' };
            } else {
                // global roamers appended to body as fixed elements
                el.classList.add('bubble-fixed');
                // low but visible z-index (below hero-content)
                el.style.zIndex = 20;
                el.style.position = 'fixed';
                el.style.left = '0px';
                el.style.top = '0px';
                document.body.appendChild(el);

                const vw = Math.max(window.innerWidth, 800);
                const vh = Math.max(window.innerHeight, 600);
                const x = Math.random() * Math.max(0, vw - cfg.w);
                const y = Math.random() * Math.max(0, vh - cfg.h);

                el._state = { x, y, vx: randomVelocity(), vy: randomVelocity(), last: null, rot: Math.random() * 360, spin: cfg.spin || 6, speedMul: cfg.speed || 0.35, w: cfg.w, h: cfg.h, zone: 'global' };
            }

            return el;
        }

        // create elements
        let elements = BUBBLES.map((cfg, i) => createBubble(cfg, i));

        // small debug: ensure bubbles were created
        console.info('bubbles: created', elements.length);

        // animation
        let rafId = null;
        function step(ts) {
            const containerRect = root.getBoundingClientRect();
            const vw = window.innerWidth;
            const vh = window.innerHeight;

            elements.forEach(el => {
                const s = el._state;
                if (!s.last) s.last = ts;
                const dt = ts - s.last;
                s.last = ts;

                s.x += s.vx * dt * s.speedMul;
                s.y += s.vy * dt * s.speedMul;
                s.rot += (s.spin * dt) / 1000;

                let maxX, maxY, bufferX, bufferY;

                if (s.zone === 'right') {
                    maxX = Math.max(0, containerRect.width - s.w);
                    maxY = Math.max(0, containerRect.height - s.h);
                    bufferX = s.w * 0.12;
                    bufferY = s.h * 0.12;
                    if (s.x <= -bufferX) { s.x = -bufferX; s.vx = Math.abs(s.vx); }
                    if (s.x >= maxX + bufferX) { s.x = maxX + bufferX; s.vx = -Math.abs(s.vx); }
                    if (s.y <= -bufferY) { s.y = -bufferY; s.vy = Math.abs(s.vy); }
                    if (s.y >= maxY + bufferY) { s.y = maxY + bufferY; s.vy = -Math.abs(s.vy); }
                    el.style.transform = `translate3d(${Math.round(s.x)}px, ${Math.round(s.y)}px, 0) rotate(${Math.round(s.rot)}deg)`;
                } else {
                    maxX = Math.max(0, vw - s.w);
                    maxY = Math.max(0, vh - s.h);
                    bufferX = s.w * 0.12;
                    bufferY = s.h * 0.12;
                    if (s.x <= -bufferX) { s.x = -bufferX; s.vx = Math.abs(s.vx); }
                    if (s.x >= maxX + bufferX) { s.x = maxX + bufferX; s.vx = -Math.abs(s.vx); }
                    if (s.y <= -bufferY) { s.y = -bufferY; s.vy = Math.abs(s.vy); }
                    if (s.y >= maxY + bufferY) { s.y = maxY + bufferY; s.vy = -Math.abs(s.vy); }
                    el.style.transform = `translate3d(${Math.round(s.x)}px, ${Math.round(s.y)}px, 0) rotate(${Math.round(s.rot)}deg)`;
                }
            });

            rafId = requestAnimationFrame(step);
        }

        rafId = requestAnimationFrame(step);

        // resize handler: clamp positions
        function onResize() {
            const containerRect = root.getBoundingClientRect();
            elements.forEach(el => {
                const s = el._state;
                if (s.zone === 'right') {
                    s.x = Math.min(s.x, Math.max(0, containerRect.width - s.w));
                    s.y = Math.min(s.y, Math.max(0, containerRect.height - s.h));
                } else {
                    s.x = Math.min(s.x, Math.max(0, window.innerWidth - s.w));
                    s.y = Math.min(s.y, Math.max(0, window.innerHeight - s.h));
                }
            });
        }
        window.addEventListener('resize', onResize);

        window.addEventListener('beforeunload', function () { if (rafId) cancelAnimationFrame(rafId); });

        // helper for testing
        window.rebuildBubblePositions = function () {
            const containerRect = root.getBoundingClientRect();
            elements.forEach(el => {
                const s = el._state;
                if (s.zone === 'right') {
                    s.x = Math.random() * Math.max(0, containerRect.width - s.w);
                    s.y = Math.random() * Math.max(0, containerRect.height - s.h);
                } else {
                    s.x = Math.random() * Math.max(0, window.innerWidth - s.w);
                    s.y = Math.random() * Math.max(0, window.innerHeight - s.h);
                }
            });
        };

    });
})();



// FAQ Accordion Toggle — no internal scroll; answers hidden by default
document.querySelectorAll('.faq-question').forEach(button => {
    button.addEventListener('click', () => {
        const item = button.closest('.faq-item');
        const isActive = item.classList.contains('active');

        // Close all items
        document.querySelectorAll('.faq-item').forEach(i => {
            i.classList.remove('active');
            const q = i.querySelector('.faq-question');
            const a = i.querySelector('.faq-answer');
            if (q) q.setAttribute('aria-expanded', 'false');
            if (a) a.setAttribute('aria-hidden', 'true');
        });

        // Open clicked item if not already open
        if (!isActive) {
            item.classList.add('active');
            const q = item.querySelector('.faq-question');
            const a = item.querySelector('.faq-answer');
            if (q) q.setAttribute('aria-expanded', 'true');
            if (a) a.setAttribute('aria-hidden', 'false');

            // optionally focus the answer for keyboard users
            if (a) a.querySelector('p')?.focus?.();
        }
    });
});
