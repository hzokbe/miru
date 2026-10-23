import { mountSuspended } from '@nuxt/test-utils/runtime';
import { describe, expect, it } from 'vitest';
import { AppSection } from '#components';

describe('AppSection', () => {
  it('renders title and slot content', async () => {
    const wrapper = await mountSuspended(AppSection, {
      props: { icon: 'i-lucide-trophy', title: 'Top animes' },
      slots: { default: '<p data-testid="content">Content</p>' },
    });

    expect(wrapper.text()).toContain('Top animes');

    expect(wrapper.find('[data-testid="content"]').exists()).toBe(true);
  });
});
