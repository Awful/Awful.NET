var images = document.querySelectorAll("img:not([src*='somethingawful'])");
for (var i = 0; i < images.length; ++i) {
    images[i].classList.add('lazy');
    images[i].classList.add('lazy-fade-in');
}