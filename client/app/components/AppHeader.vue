<template>
  <UHeader
    :toggle="false"
    :ui="{ title: 'items-center gap-2' }"
    class="fixed w-full"
  >
    <template #title>
      <UIcon class="size-5 text-primary" name="i-lucide-eye" />
      <h1>Miru</h1>
    </template>
    <template #right>
      <UColorModeButton />
    </template>
  </UHeader>
</template>
