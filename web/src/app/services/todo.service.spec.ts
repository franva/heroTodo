import { TestBed } from '@angular/core/testing';
import { TodoService } from './todo.service';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';

const url = "/api/todos";

describe('TodoService', () => {
  let service: TodoService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()]
    });
    service = TestBed.inject(TodoService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    http.verify();
  });

  it('gets all todos', () => {
    const todos = [{ id: '1', title: 'Test Todo' }];
    let result: unknown;

    service.getAll().subscribe((data) => (result = data));

    const req = http.expectOne(url);
    expect(req.request.method).toBe('GET');
    req.flush(todos);

    expect(result).toEqual(todos);
  });

  it('posts a new todo', () => {
    service.add('a book').subscribe();
    
    const req = http.expectOne(url);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ title: 'a book' });
    req.flush({ id: '1', title: 'a book' });
  });

  it('deletes a todo', () => {
    service.remove('1').subscribe();

    const req = http.expectOne(`${url}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
