export const getGenderIcon = (gender: CharacterGender) => {
  switch (gender) {
    case 'Female':
      return 'i-lucide-venus';
    case 'Male':
      return 'i-lucide-mars';
    default:
      return 'i-lucide-venus-mars';
  }
};

export const fullName = (name: string, lastName: string | null) =>
  [name, lastName].filter(Boolean).join(' ');
