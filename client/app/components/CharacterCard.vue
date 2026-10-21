<script lang="ts" setup>
const props = defineProps<{
  id: string;

  name: string;

  lastName: string;

  age: string | null;

  gender: string;

  role: string;

  description: string | null;

  imageSource: string | null;
}>();

const fullName = computed(() => `${props.name} ${props.lastName ?? ''}`);
</script>

<template>
  <UCard
    :ui="{
      root: 'flex flex-col',
      header: 'flex-1',
      body: 'flex justify-center',
    }"
  >
    <template #header>
      <NuxtLink
        :to="`/characters/${id}`"
        class="hover:brightness-75 transition-all duration-200"
      >
        <h3 class="font-bold">{{ fullName }}</h3>
      </NuxtLink>
      <p class="text-muted line-clamp-2">
        {{ description ?? 'No description' }}
      </p>
    </template>
    <NuxtLink
      :to="`/characters/${id}`"
      class="hover:brightness-75 transition-all duration-200"
    >
      <AppImage :alt="fullName" :src="imageSource" />
    </NuxtLink>
    <template #footer>
      <div class="flex flex-wrap gap-2">
        <UBadge
          color="neutral"
          data-testid="character-age"
          icon="i-lucide-cake"
          variant="outline"
        >
          {{ age ?? 'Unknown' }}
        </UBadge>
        <UBadge
          :icon="
            gender == 'male'
              ? 'i-lucide-mars'
              : gender == 'female'
                ? 'i-lucide-venus'
                : 'i-lucide-venus-and-mars'
          "
          color="neutral"
          data-testid="character-gender"
          variant="outline"
        >
          {{ gender }}
        </UBadge>
        <UBadge
          color="neutral"
          data-testid="character-role"
          icon="i-lucide-tag"
          variant="outline"
        >
          {{ role }}
        </UBadge>
      </div>
    </template>
  </UCard>
</template>
