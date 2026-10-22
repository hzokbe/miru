<template>
  <UHeader
    :toggle="false"
    :ui="{ title: 'items-center gap-2' }"
    class="fixed w-full app-header-background"
  >
    <template #title>
      <Motion
        :animate="{ opacity: 1 }"
        :initial="{ opacity: 0 }"
        :transition="{
          duration: 0.75,
          ease: 'easeOut',
          delay: 0.25,
        }"
      >
        <h1
          class="bg-linear-to-r from-primary-400 via-primary-300 to-primary-500 bg-clip-text text-transparent hover:opacity-85 transition-all duration-200"
        >
          Miru
        </h1>
      </Motion>
    </template>
    <template #right>
      <Motion
        :animate="{ opacity: 1 }"
        :initial="{ opacity: 0 }"
        :transition="{
          duration: 0.75,
          ease: 'easeOut',
          delay: 0.25,
        }"
      >
        <UColorModeButton />
      </Motion>
    </template>
  </UHeader>
</template>
