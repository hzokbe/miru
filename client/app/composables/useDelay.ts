export const useDelay = (source: Ref<boolean>, delay = 200) => {
  const delayed = ref(false);

  let timer: ReturnType<typeof setTimeout> | undefined;

  watch(
    source,
    (value) => {
      clearTimeout(timer);
      if (value) {
        timer = setTimeout(() => (delayed.value = true), delay);
      } else {
        delayed.value = false;
      }
    },
    { immediate: true },
  );

  onScopeDispose(() => clearTimeout(timer));

  return delayed;
};
