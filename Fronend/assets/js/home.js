const packageContainer = document.getElementById("homePackages");
const menuButton = document.getElementById("menuToggle");
const primaryNav = document.getElementById("primaryNav");
const siteHeader = document.querySelector(".site-header");
const heroGallery = document.querySelector("[data-hero-gallery]");
const heroSlides = Array.from(heroGallery?.querySelectorAll("[data-hero-slide]") ?? []);
const heroControls = heroGallery?.parentElement?.querySelector("[data-hero-controls]");
const heroReducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)");
let activeHeroSlide = Math.max(0, heroSlides.findIndex((slide) => slide.classList.contains("is-active")));
let heroTimer = 0;
let heroPaused = false;

function availableHeroSlides() {
  return heroSlides.filter((slide) => !slide.classList.contains("is-unavailable"));
}

function showHeroSlide(index) {
  const available = availableHeroSlides();
  if (!available.length) {
    heroControls?.setAttribute("hidden", "");
    return;
  }
  const nextSlide = available[((index % available.length) + available.length) % available.length];
  activeHeroSlide = heroSlides.indexOf(nextSlide);
  heroSlides.forEach((slide, slideIndex) => {
    const isActive = slideIndex === activeHeroSlide;
    slide.classList.toggle("is-active", isActive);
    slide.setAttribute("aria-hidden", String(!isActive));
    if (!slide.classList.contains("is-unavailable")) {
      slide.setAttribute("aria-label", `Ảnh ${available.indexOf(slide) + 1} trên ${available.length}`);
    }
  });
}

function moveHeroSlide(direction) {
  const available = availableHeroSlides();
  const currentPosition = available.indexOf(heroSlides[activeHeroSlide]);
  showHeroSlide(currentPosition + direction);
}

function stopHeroAutoplay() {
  window.clearTimeout(heroTimer);
  heroTimer = 0;
}

function scheduleHeroAutoplay() {
  stopHeroAutoplay();
  if (heroPaused || heroReducedMotion.matches || availableHeroSlides().length < 2 || document.hidden) return;
  heroTimer = window.setTimeout(() => {
    moveHeroSlide(1);
    scheduleHeroAutoplay();
  }, 5000);
}

if (heroSlides.length) {
  showHeroSlide(activeHeroSlide);
  heroControls?.querySelector("[data-hero-prev]")?.addEventListener("click", () => {
    moveHeroSlide(-1);
    scheduleHeroAutoplay();
  });
  heroControls?.querySelector("[data-hero-next]")?.addEventListener("click", () => {
    moveHeroSlide(1);
    scheduleHeroAutoplay();
  });
  const heroSection = heroGallery?.parentElement;
  heroSection?.addEventListener("focusin", () => { heroPaused = true; stopHeroAutoplay(); });
  heroSection?.addEventListener("focusout", (event) => {
    if (!(event.relatedTarget instanceof Node) || !heroSection.contains(event.relatedTarget)) {
      heroPaused = false;
      scheduleHeroAutoplay();
    }
  });
  heroReducedMotion.addEventListener?.("change", scheduleHeroAutoplay);
  document.addEventListener("visibilitychange", scheduleHeroAutoplay);
  scheduleHeroAutoplay();
}

let scrollFramePending = false;
function updateScrollEffects() {
  if (scrollFramePending) return;
  scrollFramePending = true;
  window.requestAnimationFrame(() => {
    const maxScroll = document.documentElement.scrollHeight - window.innerHeight;
    const progress = maxScroll > 0 ? Math.min(window.scrollY / maxScroll, 1) : 0;
    siteHeader?.classList.toggle("is-scrolled", window.scrollY > 12);
    siteHeader?.style.setProperty("--scroll-progress", String(progress));
    scrollFramePending = false;
  });
}
window.addEventListener("scroll", updateScrollEffects, { passive: true });
updateScrollEffects();

function setMenuOpen(open) {
  menuButton?.setAttribute("aria-expanded", String(open));
  menuButton?.setAttribute("aria-label", open ? "Đóng menu điều hướng" : "Mở menu điều hướng");
  primaryNav?.classList.toggle("is-open", open);
}

menuButton?.addEventListener("click", () => {
  setMenuOpen(menuButton.getAttribute("aria-expanded") !== "true");
});

primaryNav?.addEventListener("click", (event) => {
  if (event.target instanceof Element && event.target.closest("a")) setMenuOpen(false);
});

document.addEventListener("keydown", (event) => {
  if (event.key === "Escape" && menuButton?.getAttribute("aria-expanded") === "true") {
    setMenuOpen(false);
    menuButton.focus();
  }
});

document.addEventListener("click", (event) => {
  if (
    menuButton?.getAttribute("aria-expanded") === "true" &&
    event.target instanceof Node &&
    !menuButton.contains(event.target) &&
    !primaryNav?.contains(event.target)
  ) setMenuOpen(false);
});

document.querySelectorAll("img[data-fallback]").forEach((image) => {
  image.addEventListener("error", () => {
    image.hidden = true;
    image.closest("figure")?.classList.add("image-unavailable");
    image.closest(".hero, .closing-section")?.classList.add("image-unavailable");
    const slide = image.closest("[data-hero-slide]");
    if (slide) {
      slide.classList.add("is-unavailable");
      if (slide.classList.contains("is-active")) moveHeroSlide(1);
      scheduleHeroAutoplay();
    }
  }, { once: true });
});

const year = document.getElementById("copyrightYear");
if (year) year.textContent = String(new Date().getFullYear());

function loadScript(src) {
  return new Promise((resolve) => {
    const script = document.createElement("script");
    script.src = src;
    script.async = true;
    script.onload = () => resolve(true);
    script.onerror = () => resolve(false);
    document.head.append(script);
  });
}

async function initializeScrollMotion() {
  if (scrollMotionStarted) return;
  scrollMotionStarted = true;
  if (!window.gsap && !(await loadScript("https://cdnjs.cloudflare.com/ajax/libs/gsap/3.13.0/gsap.min.js"))) return;
  if (!window.ScrollTrigger && !(await loadScript("https://cdnjs.cloudflare.com/ajax/libs/gsap/3.13.0/ScrollTrigger.min.js"))) return;
  const gsap = window.gsap;
  const ScrollTrigger = window.ScrollTrigger;
  if (!gsap || !ScrollTrigger) return;

  if (document.fonts?.ready) {
    await Promise.race([document.fonts.ready, new Promise((resolve) => window.setTimeout(resolve, 1200))]);
  }

  let motionContext;
  let desktopMedia;
  let motionMedia;
  try {
    gsap.registerPlugin(ScrollTrigger);
    motionMedia = gsap.matchMedia();
    motionMedia.add("(prefers-reduced-motion: no-preference)", () => {
      const context = gsap.context(() => {
        const hero = document.querySelector(".hero");
        const heroImages = gsap.utils.toArray(".hero-image");
        const heroImage = heroImages[0];
        const heroLines = gsap.utils.toArray(".hero-line");
        const heroKicker = document.querySelector(".hero-kicker");
        const heroCopy = document.querySelector(".hero-copy");
        const heroPrimary = document.querySelector(".hero-primary-cta");
        const heroSecondary = document.querySelector(".hero-secondary-cta");
        if (heroLines.length && heroKicker && heroCopy && heroPrimary && heroSecondary && window.scrollY < 80) {
          const heroIntro = gsap.timeline({ defaults: { ease: "power4.out" } });
          if (heroImage) {
            heroIntro.fromTo(heroImage, { clipPath: "inset(100% 0 0 0)" }, {
              clipPath: "inset(0% 0 0 0)",
              duration: 1.25,
              ease: "power3.inOut"
            }, 0);
          }
          heroIntro
            .fromTo(heroKicker, { opacity: 0, y: 14 }, { opacity: 1, y: 0, duration: 0.45 })
            .fromTo(heroLines[0], { yPercent: 112 }, { yPercent: 0, duration: 0.82 }, 0.26)
            .fromTo(heroLines[1], { yPercent: 112 }, { yPercent: 0, duration: 0.86 }, 0.42)
            .fromTo(heroCopy, { opacity: 0, y: 18 }, { opacity: 1, y: 0, duration: 0.5 }, 1.34)
            .fromTo(heroPrimary, { y: 15 }, { y: 0, duration: 0.45 }, 1.52)
            .fromTo(heroSecondary, { y: 12 }, { y: 0, duration: 0.42 }, 1.64);
        }

        gsap.utils.toArray(".editorial-image[data-reveal] img, .closing-section > img").forEach((image) => {
          const revealTarget = image.closest(".editorial-image, .closing-section");
          gsap.fromTo(image, { clipPath: "inset(100% 0 0 0)", yPercent: 6, scale: 1.025 }, {
            clipPath: "inset(0% 0 0 0)",
            yPercent: 0,
            scale: 1,
            duration: 0.95,
            ease: "power3.inOut",
            scrollTrigger: {
              trigger: revealTarget,
              start: "top 82%",
              once: true
            }
          });
        });

        gsap.utils.toArray(".story-heading .display-title, .intro-copy h2, .section-head .display-title, .space-heading h2, .steps-intro .display-title, .principles-layout > div:first-child .display-title, .faq-intro h2, .closing-content h2").forEach((heading) => {
          gsap.fromTo(heading, { opacity: 0, y: 24 }, {
            opacity: 1,
            y: 0,
            duration: 0.75,
            ease: "power3.out",
            scrollTrigger: { trigger: heading, start: "top 84%", once: true }
          });
        });

        desktopMedia = gsap.matchMedia();
        desktopMedia.add("(min-width: 861px) and (min-height: 680px)", () => {
          const heroImages = gsap.utils.toArray(".hero-image");
          if (hero && heroImages.length) {
            gsap.to(heroImages, {
              scale: 1.14,
              yPercent: 5,
              ease: "none",
              scrollTrigger: {
                trigger: hero,
                start: "top top",
                end: "bottom top",
                scrub: 0.8
              }
            });
          }

          const storyTrack = document.querySelector(".story-track");
          const storyStage = document.querySelector(".story-stage");
          const storyVisual = document.querySelector(".story-visual");
          const storyImages = storyVisual ? Array.from(storyVisual.querySelectorAll(":scope > img")) : [];
          const storySteps = Array.from(document.querySelectorAll("[data-story-step]"));
          if (!storyTrack || !storyStage || !storyVisual || storyImages.length !== 3 || storySteps.length !== 3) return;

          storyVisual.classList.add("is-animated");
          gsap.set(storyImages, { autoAlpha: 0, scale: 1.035 });
          gsap.set(storyImages[0], { autoAlpha: 1 });
          const activateStoryStep = (activeIndex) => {
            storySteps.forEach((step, index) => step.classList.toggle("is-current", index === activeIndex));
            gsap.to(storyImages, {
              autoAlpha: (index) => index === activeIndex ? 1 : 0,
              duration: 0.65,
              ease: "sine.inOut",
              overwrite: "auto"
            });
          };

          const viewportMarker = window.innerHeight * 0.58;
          let initialStep = 0;
          storySteps.forEach((step, index) => {
            if (step.getBoundingClientRect().top <= viewportMarker) initialStep = index;
          });
          activateStoryStep(initialStep);

          storySteps.forEach((step, index) => {
            ScrollTrigger.create({
              trigger: step,
              start: "top 58%",
              end: "bottom 42%",
              onEnter: () => activateStoryStep(index),
              onEnterBack: () => activateStoryStep(index)
            });
          });

          return () => {
            storyVisual.classList.remove("is-animated");
            storySteps.forEach((step, index) => step.classList.toggle("is-current", index === 0));
          };
        });
      });
      motionContext = context;
      return () => {
        desktopMedia?.revert();
        context.revert();
      };
    });
    const refreshTriggers = () => window.requestAnimationFrame(() => ScrollTrigger.refresh());
    if (document.readyState === "complete") refreshTriggers();
    else window.addEventListener("load", refreshTriggers, { once: true });
  } catch {
    desktopMedia?.revert();
    motionContext?.revert();
    motionMedia?.revert();
    document.querySelectorAll(".story-visual").forEach((visual) => visual.classList.remove("is-animated"));
    document.querySelectorAll(".story-step").forEach((step) => {
      step.classList.toggle("is-current", step === document.querySelector("[data-story-step]"));
    });
    document.querySelectorAll(".hero-line, .story-line, .hero-kicker, .hero-copy, .hero-primary-cta, .hero-secondary-cta, .story-heading .display-title, .intro-copy h2, .section-head .display-title, .space-heading h2, .steps-intro .display-title, .principles-layout > div:first-child .display-title, .faq-intro h2, .closing-content h2, .editorial-image img, .closing-section > img, .story-visual > img, .hero-image").forEach((element) => {
      element.style.removeProperty("opacity");
      element.style.removeProperty("visibility");
      element.style.removeProperty("transform");
      element.style.removeProperty("clip-path");
    });
  }
}

let scrollMotionStarted = false;
initializeScrollMotion();

const vnd = new Intl.NumberFormat("vi-VN", {
  style: "currency",
  currency: "VND",
  maximumFractionDigits: 0
});

function packageState(message, kind = "empty", retry = false) {
  const state = document.createElement("div");
  state.className = `package-state package-state-${kind}`;
  const text = document.createElement("span");
  text.textContent = message;
  state.append(text);
  if (retry) {
    const button = document.createElement("button");
    button.type = "button";
    button.className = "package-retry";
    button.textContent = "Thử tải lại";
    button.addEventListener("click", loadPackages);
    state.append(button);
  }
  packageContainer.replaceChildren(state);
}

function createPackageCard(item, index) {
  const card = document.createElement("article");
  card.className = "package-card";
  card.style.setProperty("--card-index", String(index));

  const top = document.createElement("div");
  top.className = "package-card-top";
  const label = document.createElement("span");
  label.className = "package-label";
  label.textContent = "Gói tập";
  const id = document.createElement("span");
  id.className = "package-id";
  id.textContent = `Mã ${item.id}`;
  top.append(label, id);

  const name = document.createElement("h3");
  name.className = "package-name";
  name.textContent = item.name;
  const price = document.createElement("p");
  price.className = "package-price";
  price.textContent = vnd.format(item.price);
  const duration = document.createElement("p");
  duration.className = "package-duration";
  const icon = document.createElement("i");
  icon.className = "bi bi-calendar3";
  icon.setAttribute("aria-hidden", "true");
  const durationText = document.createElement("span");
  durationText.textContent = `${item.duration_in_days} ngày`;
  duration.append(icon, durationText);

  const footer = document.createElement("div");
  footer.className = "package-card-footer";
  const note = document.createElement("span");
  note.textContent = "Thông tin theo hệ thống";
  const cta = document.createElement("a");
  cta.className = "package-cta";
  cta.href = "dangky.html#formKhachDangKy";
  cta.append(document.createTextNode("Tạo hồ sơ"));
  const arrow = document.createElement("i");
  arrow.className = "bi bi-arrow-up-right";
  arrow.setAttribute("aria-hidden", "true");
  cta.append(arrow);
  footer.append(note, cta);
  card.append(top, name, price, duration, footer);
  return card;
}

let packageRequestInFlight = false;
async function loadPackages() {
  if (!packageContainer || packageRequestInFlight) return;
  packageRequestInFlight = true;
  packageContainer.setAttribute("aria-busy", "true");
  const loading = document.createElement("div");
  loading.className = "package-state package-state-loading";
  const spinner = document.createElement("span");
  spinner.className = "loading-ring";
  spinner.setAttribute("aria-hidden", "true");
  const message = document.createElement("span");
  message.textContent = "Đang tải danh sách gói tập…";
  loading.append(spinner, message);
  packageContainer.replaceChildren(loading);

  try {
    const response = await fetch("/api/Package", { headers: { Accept: "application/json" } });
    if (!response.ok) throw new Error(`HTTP ${response.status}`);
    const result = await response.json();
    if (result?.status !== "success" || !Array.isArray(result.data)) {
      throw new Error("Invalid package response");
    }

    const packages = result.data;
    const validPackages = packages.every((item) =>
      item && Number.isInteger(item.id) && typeof item.name === "string" && item.name.trim() &&
      Number.isFinite(item.price) && item.price >= 0 &&
      Number.isInteger(item.duration_in_days) && item.duration_in_days > 0
    );

    if (!validPackages) throw new Error("Invalid package fields");
    if (packages.length === 0) {
      packageState("Hiện chưa có gói tập khả dụng để hiển thị.");
    } else {
      packageContainer.replaceChildren(...packages.map(createPackageCard));
    }
  } catch {
    packageState("Không tải được danh sách gói tập. Vui lòng thử lại.", "error", true);
  } finally {
    packageRequestInFlight = false;
    packageContainer.setAttribute("aria-busy", "false");
  }
}

loadPackages();
