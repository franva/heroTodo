import { TestBed } from '@angular/core/testing';
import { App } from './app';
import { of, throwError } from 'rxjs';
import { TodoService } from './services/todo.service';

describe('App', () => {
  const cocaCola = {id: '1', title: 'Coca Cola'};
  const beef = {id: '2', title: 'Scotch Fillet'};
  let service: {getAll: any, add: any, remove: any};

  async function render () {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        {
          provide: TodoService,
          useValue: service
        }
      ]
    }).compileComponents();

    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    return {fixture, el: fixture.nativeElement as HTMLElement};
  }

  function items(el: HTMLElement) {
    return Array.from(el.querySelectorAll('li span')).map(s => s.textContent?.trim());
  }

  async function submit(
    fixture: {whenStable(): Promise<unknown>},
    el: HTMLFormElement,
    title: string
  ) {
    const input = el.querySelector('input') as HTMLInputElement;
    input.value = title;
    input.dispatchEvent(new Event('input'));
    await fixture.whenStable();
    el.querySelector('form')!.dispatchEvent(new Event('submit'));
    await fixture.whenStable();
  }

  beforeEach(async () => {
    service = { getAll: vi.fn(() => of([cocaCola])), add: vi.fn(() => of(cocaCola)), remove: vi.fn(() => of(void 0)) };
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('shows the todo itesm from the API', async () => {
    const {el} = await render();

    expect(items(el)).toEqual(['Coca Cola']);
  });

  it('show a message when the list is empty', async () => {
    service.getAll = vi.fn(() => of([]));
    const {el} = await render();

    expect(el.querySelector('.empty')?.textContent).toContain('So blank...');
  });
  
  it('adds a todo item and clears the input', async () => {
    service.add.mockReturnValue(of(beef));
    const {fixture, el} = await render();

    await submit(fixture, el as HTMLFormElement, 'Scotch Fillet');

    expect(service.add).toHaveBeenCalledWith('Scotch Fillet');
    expect(items(el)).toEqual(['Coca Cola', 'Scotch Fillet']);
    expect((el.querySelector('input') as HTMLInputElement).value).toBe('');
  });

  it('does not call the api for a blank title', async () => {
    const {fixture, el} = await render();

    await submit(fixture, el as HTMLFormElement, '  ');

    expect(service.add).not.toHaveBeenCalled();
  });

  it('deletes an item', async () => {
    service.remove.mockReturnValue(of(undefined));

    const {fixture, el} = await render();

    el.querySelector('li button')!.dispatchEvent(new Event('click'));
    await fixture.whenStable();

    expect(service.remove).toHaveBeenCalledWith('1');
    expect(items(el)).toEqual([]);
  });

  it('shows an error when the api call fails', async () => {
    service.add.mockReturnValue(throwError(() =>of('buga')));
    const {fixture, el} = await render();

    await submit(fixture, el as HTMLFormElement, 'Scotch Fillet');

    expect(el.querySelector('.error')?.textContent).toContain('Failed to add todo');
    expect(items(el)).toEqual(['Coca Cola']);
  });
});
