<template>
  <div>
    <AppHeader />
    <UMain
      class="flex flex-col items-center justify-start min-h-dvh app-background"
    >
      <slot />
    </UMain>
    <AppFooter />
  </div>
</template>
