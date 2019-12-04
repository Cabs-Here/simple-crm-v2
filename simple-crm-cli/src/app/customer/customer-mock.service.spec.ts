import { TestBed } from '@angular/core/testing';

import { CustomerMockService } from './customer-mock.service';

describe('CustomerMockService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CustomerMockService = TestBed.get(CustomerMockService);
    expect(service).toBeTruthy();
  });
});
