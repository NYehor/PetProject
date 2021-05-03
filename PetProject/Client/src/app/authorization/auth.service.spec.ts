import { TestBed } from '@angular/core/testing';

import { AuthorizeService } from './auth.service';

describe('AuthService', () => {
  let service: AuthorizeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthorizeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
