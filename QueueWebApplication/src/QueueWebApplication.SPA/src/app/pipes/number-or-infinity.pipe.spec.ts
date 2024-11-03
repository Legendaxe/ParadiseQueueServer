import { NumberOrInfinityPipe } from './number-or-infinity.pipe';

describe('NumberOrInfinityPipe', () => {
  it('create an instance', () => {
    const pipe = new NumberOrInfinityPipe();
    expect(pipe).toBeTruthy();
  });
});
