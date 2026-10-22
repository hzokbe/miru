<template>
  <USeparator class="h-px" />
  <UFooter>
    <template #left>
      <p class="text-muted text-sm">
        © {{ new Date().getFullYear() }} Miru. All rights reserved.
      </p>
    </template>
    <template #right>
      <UButton
        aria-label="Miru on GitHub"
        color="neutral"
        icon="i-simple-icons-github"
        target="_blank"
        to="https://github.com/hzokbe/miru"
        variant="ghost"
      />
    </template>
  </UFooter>
</template>
