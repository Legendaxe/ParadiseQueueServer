import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'numberOrInfinity',
  standalone: true
})
export class NumberOrInfinityPipe implements PipeTransform {

  transform(value: number): string {
    if (value == -1) {
      return '∞';
    }
    else {
      return value.toString();
    }
  }
}
