import { TestBed } from '@angular/core/testing';

import { MockServersServiceService } from './mock-servers-service.service';

describe('MockServersServiceService', () => {
  let service: MockServersServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MockServersServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
