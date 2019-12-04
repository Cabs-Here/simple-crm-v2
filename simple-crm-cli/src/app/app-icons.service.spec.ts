import { TestBed } from '@angular/core/testing';

import { AppIconsService } from './app-icons.service';

describe('AppIconsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AppIconsService = TestBed.get(AppIconsService);
    expect(service).toBeTruthy();
  });
});
