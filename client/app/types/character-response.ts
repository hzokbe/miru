export type CharacterGender = 'Female' | 'Male' | 'Other' | 'Unknown';

export interface CharacterResponse {
  id: string;

  name: string;

  lastName: string | null;

  japaneseName: string | null;

  japaneseLastName: string | null;

  age: string | null;

  gender: CharacterGender;

  role: string;

  description: string | null;

  imageSource: string | null;
}
