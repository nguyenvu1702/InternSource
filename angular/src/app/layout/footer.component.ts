import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FooterComponent extends AppComponentBase {
  currentYear: number;
  versionText: string;

  constructor(injector: Injector) {
    super(injector);
    this.currentYear = new Date().getFullYear();

    this.versionText = '0.0.0.1 [02022021]';
  }
}
