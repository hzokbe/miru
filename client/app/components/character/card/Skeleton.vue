<template>
  <AppCardSkeleton
    :badges="[
      {
        id: 'character-role',
        text: 'Main',
        color: 'neutral',
        icon: 'i-lucide-tag',
      },
      {
        id: 'character-gender',
        text: 'Female',
        color: 'neutral',
        icon: 'i-lucide-venus',
      },
      {
        id: 'character-age',
        text: '17',
        color: 'neutral',
        icon: 'i-lucide-cake',
      },
    ]"
    :image="{ alt: 'Kaguya Shinomiya', src: null }"
    title="Kaguya Shinomiya"
  />
</template>
