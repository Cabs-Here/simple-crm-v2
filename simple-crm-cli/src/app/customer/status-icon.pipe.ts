import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusIcon'
})
export class StatusIconPipe implements PipeTransform {

  transform(value: any, ...args: any[]): any {
    value = value || '';
    if (value.search(/prospect/i) === 0) {
      return 'online';
    }
    if (value.search(/purchased/i) === 0) {
      return 'money';
    }
    return 'users';
  }
}
