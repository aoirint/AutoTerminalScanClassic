# Developer Documentation

## Documentation boundaries

- `domain/` contains versioned Lethal Company and reusable implementation
  knowledge. It must not prescribe this mod's product behaviour, model, or
  design choices.
- `architecture/` contains this mod's models, workflows, responsibilities,
  and design decisions. It links to the domain knowledge it relies on.
- `operations/` contains repeatable maintainer procedures. It does not own
  either base-game facts or mod design decisions.
- Keep a domain document focused on one game or technical concern. Add a new
  domain document when an architecture document needs knowledge not already
  covered there.
- Keep an architecture document focused on one mod concern. Do not copy
  base-game member declarations or behaviour analysis into it; link to the
  relevant domain document instead.

## Release assets

- [Icon authoring](release/icon-authoring.md) describes the package icon source
  and regeneration workflow.

Start with [architecture/README.md](architecture/README.md) for the mod design,
[domain/README.md](domain/README.md) for supporting knowledge, and
[operations/README.md](operations/README.md) for maintainer procedures.
