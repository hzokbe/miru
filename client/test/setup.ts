const originalAnimate = Element.prototype.animate;

Element.prototype.animate = function (
  ...args: Parameters<typeof originalAnimate>
) {
  const animation = originalAnimate.apply(this, args);

  animation.finished.catch(() => {});

  return animation;
};
